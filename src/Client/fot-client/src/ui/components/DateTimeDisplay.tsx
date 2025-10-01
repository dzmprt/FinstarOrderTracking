import { formatDateTime } from '../../application/utils/dateUtils';

interface DateTimeDisplayProps {
  dateString: string;
  fallback?: string;
  className?: string;
}

export const DateTimeDisplay: React.FC<DateTimeDisplayProps> = ({ 
  dateString, 
  fallback = '-', 
  className = '' 
}) => {
  if (!dateString) {
    return <span className={className}>{fallback}</span>;
  }

  return <span className={className}>{formatDateTime(dateString)}</span>;
};
