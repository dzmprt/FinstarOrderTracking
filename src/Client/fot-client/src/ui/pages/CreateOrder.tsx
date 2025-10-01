import { useState } from 'react';
import { useAppDispatch } from '../../application/hooks';
import { createOrder } from '../../application/order/ordersSlice';

export const CreateOrder = () => {
    const dispatch = useAppDispatch();
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

    return (
        <div className="container my-4 px-3">
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
    );
};
