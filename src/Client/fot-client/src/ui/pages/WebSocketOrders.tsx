import { useAppDispatch, useAppSelector } from '../../application/hooks';
import { clearOrders } from '../../application/websocket/websocketSlice';
import { StatusBadge } from '../components/StatusBadge';
import { DateTimeDisplay } from '../components/DateTimeDisplay';

export const WebSocketOrders: React.FC = () => {
  const dispatch = useAppDispatch();
  const { orders, error } = useAppSelector(s => s.websocket);

  const handleClearOrders = () => {
    dispatch(clearOrders());
  };

  // Status utilities are now imported from utils

  return (
    <div className="container my-4 px-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2 className="h4 mb-0">Order Status Changes</h2>
        <button className="btn btn-outline-secondary btn-sm" onClick={handleClearOrders}>
          Clear
        </button>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      <div className="card border-0 shadow-sm">
        <div className="card-body">
          {orders.length === 0 ? (
            <div className="text-center text-muted py-4">
              Waiting for order status changes from WebSocket...
            </div>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover align-middle">
                <thead className="table-light">
                  <tr>
                    <th>Order #</th>
                    <th>New Status</th>
                    <th>Updated At</th>
                    <th>Received At</th>
                  </tr>
                </thead>
                <tbody>
                  {orders.map((order, index) => (
                    <tr key={`${order.orderNumber}-${index}`}>
                      <td className="fw-medium">{order.orderNumber}</td>
                      <td>
                        <StatusBadge status={order.status} />
                      </td>
                      <td>
                        <DateTimeDisplay dateString={order.updatedAt} />
                      </td>
                      <td className="text-muted small">
                        <DateTimeDisplay dateString={order.receivedAt} />
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};