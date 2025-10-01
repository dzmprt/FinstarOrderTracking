export interface OrderStatusChangedEvent {
  orderNumber: string;
  status: number;
  updatedAt: string;
}