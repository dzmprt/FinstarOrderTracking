import { getStatusLabel, getStatusBadgeClass } from '../../application/utils/statusUtils';

interface StatusBadgeProps {
  status: number;
  className?: string;
}

export const StatusBadge: React.FC<StatusBadgeProps> = ({ status, className = '' }) => {
  return (
    <span className={`badge ${getStatusBadgeClass(status)} ${className}`}>
      {getStatusLabel(status)}
    </span>
  );
};
