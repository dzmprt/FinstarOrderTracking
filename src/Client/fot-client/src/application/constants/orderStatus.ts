export const ORDER_STATUS = {
  CREATED: 1,
  SHIPPED: 2,
  DELIVERED: 3,
  CANCELLED: 4,
} as const;

export const ORDER_STATUS_LABELS = {
  [ORDER_STATUS.CREATED]: 'Created',
  [ORDER_STATUS.SHIPPED]: 'Shipped',
  [ORDER_STATUS.DELIVERED]: 'Delivered',
  [ORDER_STATUS.CANCELLED]: 'Cancelled',
} as const;

export const ORDER_STATUS_BADGE_CLASSES = {
  [ORDER_STATUS.CREATED]: 'text-bg-secondary',
  [ORDER_STATUS.SHIPPED]: 'text-bg-warning',
  [ORDER_STATUS.DELIVERED]: 'text-bg-success',
  [ORDER_STATUS.CANCELLED]: 'text-bg-danger',
} as const;

export type OrderStatus = typeof ORDER_STATUS[keyof typeof ORDER_STATUS];
