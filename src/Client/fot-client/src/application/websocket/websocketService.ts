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
        store.dispatch(setError('WebSocket URL not configured'));
        return;
      }

      this.ws = new WebSocket(wsUrl);

      this.ws.onopen = () => {
        store.dispatch(setConnectionStatus(true));
        this.reconnectAttempts = 0;
      };

      this.ws.onmessage = (event) => {
        try {
          const rawData = JSON.parse(event.data);
          // Map different possible data structures
          const statusChange: OrderStatusChangedEvent = {
            orderNumber: rawData.OrderNumber || rawData.orderNumber || rawData.order_number || rawData.id || 'Unknown',
            status: rawData.NewStatus || rawData.status || rawData.newStatus || rawData.new_status || 0,
            updatedAt: rawData.UpdatedAt || rawData.updatedAt || rawData.updated_at || rawData.timestamp || new Date().toISOString()
          };
          store.dispatch(addOrder(statusChange));
        } catch {
          store.dispatch(setError('Failed to parse WebSocket message'));
        }
      };

      this.ws.onclose = () => {
        store.dispatch(setConnectionStatus(false));
        this.scheduleReconnect();
      };

      this.ws.onerror = () => {
        store.dispatch(setError('WebSocket connection error'));
      };
    } catch {
      store.dispatch(setError('Failed to create WebSocket connection'));
    }
  }

  private scheduleReconnect(): void {
    if (this.reconnectAttempts >= this.maxReconnectAttempts) {
    // Max reconnection attempts reached
      return;
    }

    const delay = Math.min(
      WEBSOCKET_CONFIG.RECONNECT_DELAY * Math.pow(2, this.reconnectAttempts),
      WEBSOCKET_CONFIG.MAX_RECONNECT_DELAY
    );
    this.reconnectAttempts++;

    
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
