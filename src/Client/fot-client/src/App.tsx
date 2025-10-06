import './App.css';
import { Provider } from 'react-redux';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import { OrdersList } from './ui/pages/OrdersList';
import { OrderDetails } from './ui/pages/OrderDetails';
import { About } from './ui/pages/About';
import { WebSocketOrders } from './ui/pages/WebSocketOrders';
import { Header } from './ui/components/Header';
import { GlobalToast } from './ui/components/GlobalToast';
import { store } from './application/store';
import { websocketService } from './application/websocket/websocketService';
import { useEffect } from 'react';
import { CreateOrder } from './ui/pages/CreateOrder';

/**
 * Main application component for Finstar Order Tracking client.
 * Handles routing, global state, and WebSocket lifecycle.
 */
const App: React.FC = () => {
  useEffect(() => {
    websocketService.connect();
    return () => {
      websocketService.disconnect();
    };
  }, []);

  return (
    <Provider store={store}>
      <BrowserRouter>
        <Header />
        <Routes>
          <Route path="/" element={<Navigate to="/orders" replace />} />
          <Route path="/about" element={<About />} />
          <Route path="/orders" element={<OrdersList />} />
          <Route path="/orders/create" element={<CreateOrder />} />
          <Route path="/orders/:orderNumber" element={<OrderDetails />} />
          <Route path="/websocket" element={<WebSocketOrders />} />
        </Routes>
        <GlobalToast />
      </BrowserRouter>
    </Provider>
  );
};

export default App;
