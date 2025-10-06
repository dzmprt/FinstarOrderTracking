import { ORDER_STATUS_LABELS, ORDER_STATUS_BADGE_CLASSES, type OrderStatus } from '../constants/orderStatus';

/**
 * Returns the label for a given order status code.
 */
export const getStatusLabel = (status: number): string => {
  return ORDER_STATUS_LABELS[status as OrderStatus] || `Status ${status}`;
};

/**
 * Returns the Bootstrap badge class for a given order status code.
 */
export const getStatusBadgeClass = (status: number): string => {
  return ORDER_STATUS_BADGE_CLASSES[status as OrderStatus] || 'text-bg-light';
};

/**
 * Checks if a status transition is valid.
 */
export const isValidStatusTransition = (currentStatus: number, newStatus: number): boolean => {
  // Allow progression forward or cancellation from any status
  return newStatus > currentStatus || newStatus === 4; // 4 = CANCELLED
};

/**
 * Returns available next status options for a given current status.
 */
export const getNextStatusOptions = (currentStatus: number): Array<{ value: number; label: string }> => {
  const options: Array<{ value: number; label: string }> = [];
  switch (currentStatus) {
    case 1: // CREATED
      options.push(
        { value: 2, label: 'Shipped (2)' },
        { value: 4, label: 'Cancelled (4)' }
      );
      break;
    case 2: // SHIPPED
      options.push(
        { value: 3, label: 'Delivered (3)' },
        { value: 4, label: 'Cancelled (4)' }
      );
      break;
    case 3: // DELIVERED
      options.push({ value: 3, label: 'Delivered (3) - Final status' });
      break;
    case 4: // CANCELLED
      options.push({ value: 4, label: 'Cancelled (4) - Final status' });
      break;
  }
  return options;
};
