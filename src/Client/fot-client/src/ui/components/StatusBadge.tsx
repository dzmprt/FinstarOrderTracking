import { getStatusLabel, getStatusBadgeClass } from '../../application/utils/statusUtils';


/**
 * Displays a status badge for an order.
 * @param status - Order status code
 * @param className - Additional CSS classes
 */
interface StatusBadgeProps {
  status: number;
  className?: string;
}

export const StatusBadge: React.FC<StatusBadgeProps> = ({ status, className = '' }) => (
  <span
    className={`badge ${getStatusBadgeClass(status)} ${className}`}
    aria-label={`Order status: ${getStatusLabel(status)}`}
    title={getStatusLabel(status)}
    role="status"
  >
    {getStatusLabel(status)}
  </span>
);
