import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { OrderStatusChangedEvent } from './orderStatusChangedEvent';

export interface WebSocketOrderStatusChangedEvent extends OrderStatusChangedEvent {
  receivedAt: string;
}

export interface WebSocketState {
  orders: WebSocketOrderStatusChangedEvent[];
  isConnected: boolean;
  error?: string;
  notifications: Array<{
    id: string;
    orderNumber: string;
    timestamp: string;
  }>;
}

const initialState: WebSocketState = {
  orders: [],
  isConnected: false,
  error: undefined,
  notifications: [],
};

const websocketSlice = createSlice({
  name: 'websocket',
  initialState,
  reducers: {
    setConnectionStatus(state, action: PayloadAction<boolean>) {
      state.isConnected = action.payload;
      if (action.payload) {
        state.error = undefined;
      }
    },
    setError(state, action: PayloadAction<string>) {
      state.error = action.payload;
    },
    addOrder(state, action: PayloadAction<OrderStatusChangedEvent>) {
      const webSocketOrder: WebSocketOrderStatusChangedEvent = {
        ...action.payload,
        receivedAt: new Date().toISOString(),
      };
      state.orders.unshift(webSocketOrder);
      
      // Add notification with unique ID
      const notificationId = `${action.payload.orderNumber}-${Date.now()}`;
      state.notifications.push({
        id: notificationId,
        orderNumber: action.payload.orderNumber,
        timestamp: webSocketOrder.receivedAt,
      });
    },
    clearOrders(state) {
      state.orders = [];
    },
    removeNotification(state, action: PayloadAction<string>) {
      state.notifications = state.notifications.filter(n => n.id !== action.payload);
    },
    clearAllNotifications(state) {
      state.notifications = [];
    },
  },
});

export const { setConnectionStatus, setError, addOrder, clearOrders, removeNotification, clearAllNotifications } = websocketSlice.actions;
export default websocketSlice.reducer;
