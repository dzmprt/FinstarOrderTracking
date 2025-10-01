import { configureStore } from '@reduxjs/toolkit';
import ordersReducer from './order/ordersSlice';
import websocketReducer from './websocket/websocketSlice';

export const store = configureStore({
  reducer: {
    orders: ordersReducer,
    websocket: websocketReducer,
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware({
    serializableCheck: false,
    immutableCheck: false,
  }),
  devTools: true,
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;


