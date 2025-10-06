import React from 'react';
import { Link } from 'react-router-dom';
import { StatusBadge } from './StatusBadge';
import { DateTimeDisplay } from './DateTimeDisplay';
import type { OrderDto } from '../../application/api-client';

interface OrdersTableProps {
  items: OrderDto[];
  isLoading: boolean;
}

export const OrdersTable: React.FC<OrdersTableProps> = ({ items, isLoading }) => (
  <div className="table-responsive">
    <table className="table table-hover align-middle">
      <thead className="table-light">
        <tr>
          <th>Order #</th>
          <th>Description</th>
          <th>Status</th>
          <th>Created</th>
          <th>Updated</th>
        </tr>
      </thead>
      <tbody>
        {items.map(o => (
          <tr key={o.orderNumber}>
            <td><Link to={`/orders/${o.orderNumber}`}>{o.orderNumber}</Link></td>
            <td className="text-truncate" style={{ maxWidth: '32ch' }}>{o.description}</td>
            <td><StatusBadge status={o.status} /></td>
            <td><DateTimeDisplay dateString={o.createdAt} /></td>
            <td><DateTimeDisplay dateString={o.updatedAt || ''} /></td>
          </tr>
        ))}
        {items.length === 0 && (
          <tr>
            <td colSpan={5}>{isLoading ? 'Loading...' : 'No orders found'}</td>
          </tr>
        )}
      </tbody>
    </table>
  </div>
);
