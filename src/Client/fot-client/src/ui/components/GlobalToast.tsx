import { useEffect } from 'react';
import { useAppSelector, useAppDispatch } from '../../application/hooks';
import { removeNotification } from '../../application/websocket/websocketSlice';
import { WEBSOCKET_CONFIG } from '../../application/constants/websocket';

export const GlobalToast: React.FC = () => {
  const dispatch = useAppDispatch();
  const { notifications } = useAppSelector(s => s.websocket);

  const handleDismiss = (notificationId: string) => {
    dispatch(removeNotification(notificationId));
  };

  useEffect(() => {
    // Auto-dismiss notifications after 5 seconds
    notifications.forEach(notification => {
      const timer = setTimeout(() => {
        dispatch(removeNotification(notification.id));
      }, WEBSOCKET_CONFIG.NOTIFICATION_AUTO_DISMISS_DELAY);

      return () => clearTimeout(timer);
    });
  }, [notifications, dispatch]);

  if (notifications.length === 0) {
    return null;
  }

  return (
    <div className="toast-container position-fixed top-0 end-0 p-3" style={{ zIndex: 1055 }}>
      {notifications.map((notification, index) => (
        <div 
          key={notification.id}
          className="toast show mb-2" 
          role="alert" 
          aria-live="assertive" 
          aria-atomic="true"
          style={{ 
            transform: `translateY(${index * 10}px)`,
            transition: 'all 0.3s ease-in-out'
          }}
        >
          <div className="toast-header">
            <strong className="me-auto">Order Status Changed</strong>
            <button 
              type="button" 
              className="btn-close" 
              onClick={() => handleDismiss(notification.id)}
            ></button>
          </div>
          <div className="toast-body">
            Order #{notification.orderNumber} status changed at {new Date(notification.timestamp).toLocaleString()}
          </div>
        </div>
      ))}
    </div>
  );
};
