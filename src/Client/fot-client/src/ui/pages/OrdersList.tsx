import { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../application/hooks';
import { fetchOrders, setQuery, createOrder } from '../../application/order/ordersSlice';
import { Link, useSearchParams } from 'react-router-dom';
import { ApiV1OrdersGetOrderByEnum } from '../../application/api-client';
import { useDebounce } from '../../application/hooks/useDebounce';
import { StatusBadge } from '../components/StatusBadge';
import { DateTimeDisplay } from '../components/DateTimeDisplay';

export const OrdersList: React.FC = () => {
    const dispatch = useAppDispatch();
    const { items, isLoading, totalKnown } = useAppSelector(s => s.orders);
    const [params, setParams] = useSearchParams();

    // CreateOrder form state
    const [description, setDescription] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setIsSubmitting(true);
        try {
            await dispatch(createOrder({ description })).unwrap();
            setDescription('');
        } catch (err: any) {
            setError(err?.message || 'Failed to create order');
        } finally {
            setIsSubmitting(false);
        }
    };

    const [search, setSearch] = useState(params.get('q') ?? '');
    const debounced = useDebounce(search);
    const [status, setStatus] = useState<string>(params.get('status') ?? '');
    const [sortBy, setSortBy] = useState<ApiV1OrdersGetOrderByEnum>(params.get('sort') ?  Number(params.get('sort')) as ApiV1OrdersGetOrderByEnum : ApiV1OrdersGetOrderByEnum.NUMBER_1);
    const [limit, setLimit] = useState<number>(Number(params.get('limit') ?? '10'));
    const [page, setPage] = useState<number>(Number(params.get('page') ?? '1'));

    useEffect(() => {
        const offset = (page - 1) * limit;
        const statuses = status ? status.split(',').map(s => Number(s) as 1 | 2 | 3 | 4) : undefined;
        dispatch(setQuery({ limit, offset, freeText: debounced || undefined, statuses, sortBy }));
        dispatch(fetchOrders({ limit, offset, freeText: debounced || undefined, statuses, sortBy }));
    }, [dispatch, debounced, status, sortBy, limit, page]);

    useEffect(() => {
        const next = new URLSearchParams();
        if (debounced) next.set('q', debounced);
        if (status) next.set('status', status);
        if (sortBy !== ApiV1OrdersGetOrderByEnum.NUMBER_1) next.set('sort', sortBy.toString());
        if (limit !== 10) next.set('limit', String(limit));
        if (page !== 1) next.set('page', String(page));
        setParams(next, { replace: true });
    }, [debounced, status, sortBy, limit, page, setParams]);

    const canPrev = page > 1;
    const canNext = totalKnown === false || items.length === limit;

    return (
        <div className="container my-4 px-3">
            {/* CreateOrder form above filters */}
            <div className="mb-4">
                <h2 className="h5 mb-3">Create New Order</h2>
                <form onSubmit={handleSubmit} className="card border-0 shadow-sm p-3">
                    <div className="mb-3">
                        <label className="form-label">Description</label>
                        <input
                            className="form-control"
                            type="text"
                            value={description}
                            onChange={e => setDescription(e.target.value)}
                            required
                            disabled={isSubmitting}
                            placeholder="Order description"
                        />
                    </div>
                    {error && <div className="alert alert-danger">{error}</div>}
                    <button className="btn btn-primary" type="submit" disabled={isSubmitting || !description}>
                        {isSubmitting ? 'Creating...' : 'Create Order'}
                    </button>
                </form>
            </div>
            <div className="d-flex flex-column flex-lg-row gap-3">
                <aside className="filters-sidebar flex-shrink-0">
                    <div className="card border-0 shadow-sm">
                        <div className="card-body">
                            <div className="mb-3">
                                <label className="form-label">Search</label>
                                <input className="form-control" placeholder="Search..." value={search} onChange={e => { setSearch(e.target.value); setPage(1); }} />
                            </div>
                            <div className="mb-3">
                                <label className="form-label">Status</label>
                                <select multiple className="form-select" value={status ? status.split(',') : []} onChange={e => {
                                    const arr = Array.from(e.target.selectedOptions).map(o => o.value);
                                    setStatus(arr.join(',')); setPage(1);
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
                                <select className="form-select" value={sortBy} onChange={e => { setSortBy(Number(e.target.value) as ApiV1OrdersGetOrderByEnum); }}>
                                    <option value="1">By number</option>
                                    <option value="2">By status</option>
                                    <option value="3">By created at</option>
                                    <option value="4">By updated at</option>
                                </select>
                            </div>
                            <div className="mb-2">
                                <label className="form-label">Page size</label>
                                <select className="form-select" value={String(limit)} onChange={e => { setLimit(Number(e.target.value)); setPage(1); }}>
                                    <option value="5">5</option>
                                    <option value="10">10</option>
                                    <option value="20">20</option>
                                    <option value="50">50</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </aside>
                <section className="flex-grow-1">
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
                                        <td>
                                            <StatusBadge status={o.status} />
                                        </td>
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
                    <nav className="d-flex justify-content-between align-items-center" aria-label="Pagination">
                        <ul className="pagination mb-0">
                            <li className={`page-item ${!canPrev ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setPage(p => Math.max(1, p - 1))}>Prev</button>
                            </li>
                            <li className="page-item disabled"><span className="page-link">Page {page}</span></li>
                            <li className={`page-item ${!canNext ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setPage(p => p + 1)}>Next</button>
                            </li>
                        </ul>
                    </nav>
                </section>
            </div>
        </div>
    );
};


