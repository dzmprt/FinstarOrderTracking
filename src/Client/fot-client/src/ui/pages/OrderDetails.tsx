import { useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../application/hooks';
import { fetchOrderByNumber, updateOrderStatus } from '../../application/order/ordersSlice';
// getStatusLabel is used in StatusBadge component
import { useOrderStatus } from '../../application/hooks/useOrderStatus';
import { StatusBadge } from '../components/StatusBadge';
import { DateTimeDisplay } from '../components/DateTimeDisplay';

export const OrderDetails: React.FC = () => {
    const { orderNumber } = useParams();
    const dispatch = useAppDispatch();
    const { selected, isLoading, updateError, isUpdating } = useAppSelector(s => s.orders);
    const { status, setStatus, statusOptions, hasStatusChanged } = useOrderStatus(selected?.status || 0);

    useEffect(() => {
        if (orderNumber) dispatch(fetchOrderByNumber(orderNumber));
    }, [dispatch, orderNumber]);

    if (isLoading && !selected) return <div className="container my-4">Loading...</div>;
    if (!selected) return <div className="container my-4">Order not found</div>;

    return (
        <div className="container my-4">
            <div className="mb-2"><Link to="/orders">← Back to Orders</Link></div>
            <div className="card shadow-sm border-0">
                <div className="card-body">
                    {updateError && (
                        <div className="alert alert-danger" role="alert">
                            {updateError}
                        </div>
                    )}
                    <h2 className="h5 mb-3">Order #{selected.orderNumber}</h2>
                    <div className="row g-3">
                        <div className="col-12 col-md-6">
                            <label className="form-label">Description</label>
                            <div className="form-control-plaintext">{selected.description}</div>
                        </div>
                        <div className="col-12 col-md-6">
                            <label className="form-label">Current Status</label>
                            <div className="form-control-plaintext">
                                <StatusBadge status={selected.status} />
                            </div>
                        </div>
                        <div className="col-12 col-md-6">
                            <label className="form-label">Change Status To</label>
                            <select className="form-select" value={status} onChange={e => {
                                console.log('Status changed to:', e.target.value);
                                setStatus(e.target.value);
                            }}>
                                {statusOptions.map(option => (
                                    <option key={option.value} value={option.value} disabled={option.value === selected.status}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="col-12 col-md-6">
                            <label className="form-label">Created</label>
                            <div className="form-control-plaintext">
                                <DateTimeDisplay dateString={selected.createdAt} />
                            </div>
                        </div>
                        <div className="col-12 col-md-6">
                            <label className="form-label">Updated</label>
                            <div className="form-control-plaintext">
                                <DateTimeDisplay dateString={selected.updatedAt || ''} />
                            </div>
                        </div>
                        <div className="col-12">
                            <button 
                                className="btn btn-success" 
                                disabled={isUpdating || !hasStatusChanged} 
                                onClick={() => {
                                    if (orderNumber) {
                                        console.log('Updating status:', { 
                                            orderNumber, 
                                            currentStatus: selected.status,
                                            newStatus: Number(status) 
                                        });
                                        dispatch(updateOrderStatus({ orderNumber, newStatus: Number(status) as any }));
                                    }
                                }}
                            >
                                {isUpdating && <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>}
                                {hasStatusChanged ? 'Save status' : 'No changes to save'}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};


