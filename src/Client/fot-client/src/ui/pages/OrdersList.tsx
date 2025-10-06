import { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../application/hooks';
import { fetchOrders, createOrder, setQuery } from '../../application/order/ordersSlice';
import { useSearchParams } from 'react-router-dom';
import { ApiV1OrdersGetOrderByEnum } from '../../application/api-client';
import { useDebounce } from '../../application/hooks/useDebounce';
import { OrdersFilter } from '../components/OrdersFilter';
import { OrdersTable } from '../components/OrdersTable';
import { Pagination } from '../components/Pagination';

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
    const [sortBy, setSortBy] = useState<ApiV1OrdersGetOrderByEnum>(params.get('sort') ? Number(params.get('sort')) as ApiV1OrdersGetOrderByEnum : ApiV1OrdersGetOrderByEnum.NUMBER_1);
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
                <OrdersFilter
                    search={search}
                    setSearch={setSearch}
                    status={status}
                    setStatus={setStatus}
                    sortBy={sortBy}
                    setSortBy={setSortBy}
                    limit={limit}
                    setLimit={setLimit}
                />
                <section className="flex-grow-1">
                    <OrdersTable items={items} isLoading={isLoading} />
                    <Pagination page={page} canPrev={canPrev} canNext={canNext} setPage={setPage} />
                </section>
            </div>
        </div>
    );
};


