import { store } from '../store';
import { setConnectionStatus, setError, addOrder } from './websocketSlice';
import { type OrderStatusChangedEvent } from './orderStatusChangedEvent';
import { WEBSOCKET_CONFIG } from '../constants/websocket';

class WebSocketService {
  private ws: WebSocket | null = null;
  private reconnectTimeout: ReturnType<typeof setTimeout> | null = null;
  private reconnectAttempts = 0;
  private readonly maxReconnectAttempts = WEBSOCKET_CONFIG.MAX_RECONNECT_ATTEMPTS;

  connect() {
    if (this.ws?.readyState === WebSocket.OPEN) {
      return;
    }

    try {
      const wsUrl = import.meta.env.VITE_ORDER_CHANGED_WORKER_WS_URL;
      if (!wsUrl) {
        console.error('WebSocket URL not configured');
        return;
      }

      this.ws = new WebSocket(wsUrl);

      this.ws.onopen = () => {
        console.log('WebSocket connected');
        store.dispatch(setConnectionStatus(true));
        this.reconnectAttempts = 0;
      };

      this.ws.onmessage = (event) => {
        try {
          console.log('WebSocket message received:', event.data);
          const rawData = JSON.parse(event.data);
          console.log('Raw data:', rawData);
          
          // Map different possible data structures
          const statusChange: OrderStatusChangedEvent = {
            orderNumber: rawData.OrderNumber || rawData.orderNumber || rawData.order_number || rawData.id || 'Unknown',
            status: rawData.NewStatus || rawData.status || rawData.newStatus || rawData.new_status || 0,
            updatedAt: rawData.UpdatedAt || rawData.updatedAt || rawData.updated_at || rawData.timestamp || new Date().toISOString()
          };
          
          console.log('Mapped status change:', statusChange);
          store.dispatch(addOrder(statusChange));
        } catch (err) {
          console.error('Failed to parse WebSocket message:', err);
          store.dispatch(setError('Failed to parse message'));
        }
      };

      this.ws.onclose = () => {
        console.log('WebSocket disconnected');
        store.dispatch(setConnectionStatus(false));
        this.scheduleReconnect();
      };

      this.ws.onerror = (err) => {
        console.error('WebSocket error:', err);
        store.dispatch(setError('WebSocket connection error'));
      };
    } catch (err) {
      console.error('Failed to create WebSocket connection:', err);
      store.dispatch(setError('Failed to create WebSocket connection'));
    }
  }

  private scheduleReconnect(): void {
    if (this.reconnectAttempts >= this.maxReconnectAttempts) {
      console.log('Max reconnection attempts reached');
      return;
    }

    const delay = Math.min(
      WEBSOCKET_CONFIG.RECONNECT_DELAY * Math.pow(2, this.reconnectAttempts),
      WEBSOCKET_CONFIG.MAX_RECONNECT_DELAY
    );
    this.reconnectAttempts++;

    console.log(`Scheduling reconnection attempt ${this.reconnectAttempts} in ${delay}ms`);
    
    this.reconnectTimeout = setTimeout(() => {
      this.connect();
    }, delay);
  }

  disconnect() {
    if (this.reconnectTimeout) {
      clearTimeout(this.reconnectTimeout);
      this.reconnectTimeout = null;
    }

    if (this.ws) {
      this.ws.close();
      this.ws = null;
    }
  }

  isConnected(): boolean {
    return this.ws?.readyState === WebSocket.OPEN;
  }
}

export const websocketService = new WebSocketService();
