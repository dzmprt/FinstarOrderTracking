import React from 'react';

interface OrdersFilterProps {
  search: string;
  setSearch: (v: string) => void;
  status: string;
  setStatus: (v: string) => void;
  sortBy: import('../../application/api-client').ApiV1OrdersGetOrderByEnum;
  setSortBy: (v: import('../../application/api-client').ApiV1OrdersGetOrderByEnum) => void;
  limit: number;
  setLimit: (v: number) => void;
  // removed unused page and setPage props
}

export const OrdersFilter: React.FC<OrdersFilterProps> = ({
  search, setSearch, status, setStatus, sortBy, setSortBy, limit, setLimit
}) => (
  <aside className="filters-sidebar flex-shrink-0">
    <div className="card border-0 shadow-sm">
      <div className="card-body">
        <div className="mb-3">
          <label className="form-label">Search</label>
          <input className="form-control" placeholder="Search..." value={search} onChange={e => { setSearch(e.target.value); }} />
        </div>
        <div className="mb-3">
          <label className="form-label">Status</label>
          <select multiple className="form-select" value={status ? status.split(',') : []} onChange={e => {
            const arr = Array.from(e.target.selectedOptions).map(o => o.value);
            setStatus(arr.join(','));
          }}>
            <option value="1">Created</option>
            <option value="2">Shipped</option>
            <option value="3">Delivered</option>
            <option value="4">Cancelled</option>
          </select>
          <div className="form-text">Cmd/Ctrl for multiple selection</div>
        </div>
        <div className="mb-3">
          <label className="form-label">Sort</label>
          <select className="form-select" value={sortBy} onChange={e => { setSortBy(Number(e.target.value) as import('../../application/api-client').ApiV1OrdersGetOrderByEnum); }}>
            <option value="1">By number</option>
            <option value="2">By status</option>
            <option value="3">By created at</option>
            <option value="4">By updated at</option>
          </select>
        </div>
        <div className="mb-2">
          <label className="form-label">Page size</label>
          <select className="form-select" value={String(limit)} onChange={e => { setLimit(Number(e.target.value)); }}>
            <option value="5">5</option>
            <option value="10">10</option>
            <option value="20">20</option>
            <option value="50">50</option>
          </select>
        </div>
      </div>
    </div>
  </aside>
);
