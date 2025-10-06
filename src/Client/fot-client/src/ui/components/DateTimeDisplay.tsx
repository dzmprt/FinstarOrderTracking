import { formatDateTime } from '../../application/utils/dateUtils';


/**
 * Displays a formatted date/time string.
 * @param dateString - ISO date string
 * @param fallback - Fallback text if date is missing
 * @param className - Additional CSS classes
 */
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
    return <span className={className} aria-label="No date">{fallback}</span>;
  }
  return <span className={className} aria-label={formatDateTime(dateString)}>{formatDateTime(dateString)}</span>;
};
