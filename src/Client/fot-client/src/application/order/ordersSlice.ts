import { createAsyncThunk, createSlice, type PayloadAction } from '@reduxjs/toolkit';
import { OrdersApi, type OrderDto, type UpdateOrderStatusCommand, ApiV1OrdersGetOrderStatusEnum, ApiV1OrdersGetOrderByEnum, Configuration } from '../../application/api-client';
import type { AxiosError } from 'axios';

const config = new Configuration({ basePath: import.meta.env.VITE_API_BASE_URL });

const api = new OrdersApi(config);

export interface OrdersQuery {
    limit: number;
    offset?: number;
    freeText?: string;
    statuses?: ApiV1OrdersGetOrderStatusEnum[];
    sortBy?: ApiV1OrdersGetOrderByEnum;
}

export interface OrdersState {
    items: OrderDto[];
    isLoading: boolean;
    error?: string;
    selected?: OrderDto;
    totalKnown?: boolean; // true if there are no more results beyond current page
    lastQuery: OrdersQuery;
    updateError?: string;
    isUpdating?: boolean;
}

const initialState: OrdersState = {
    items: [],
    isLoading: false,
    error: undefined,
    selected: undefined,
    totalKnown: undefined,
    lastQuery: { limit: 10, offset: 0 },
    updateError: undefined,
    isUpdating: false,
};

export const fetchOrders = createAsyncThunk(
    'orders/fetchOrders',
    async (query: OrdersQuery) => {
        const { limit, offset, freeText, statuses, sortBy } = query;
        let orderBy: ApiV1OrdersGetOrderByEnum | undefined = undefined;
        const key = `${sortBy ?? ''}`;
        const map: Record<string, ApiV1OrdersGetOrderByEnum> = {
            1: ApiV1OrdersGetOrderByEnum.NUMBER_1,
            2: ApiV1OrdersGetOrderByEnum.NUMBER_2,
            3: ApiV1OrdersGetOrderByEnum.NUMBER_3,
            4: ApiV1OrdersGetOrderByEnum.NUMBER_4,
        };
        if (map[key]) orderBy = map[key];
        const response = await api.apiV1OrdersGet(
            limit,
            offset,
            freeText,
            (statuses as ApiV1OrdersGetOrderStatusEnum[] | undefined),
            undefined,
            undefined,
            undefined,
            undefined,
            orderBy
        );
        const data = response.data;
        return { data, query };
    }
);

export const fetchOrderByNumber = createAsyncThunk(
    'orders/fetchOrderByNumber',
    async (orderNumber: string) => {
        const response = await api.apiV1OrdersOrderNumberGet(orderNumber);
        return response.data;
    }
);

export const updateOrderStatus = createAsyncThunk<
    OrderDto,
    { orderNumber: string; newStatus: ApiV1OrdersGetOrderStatusEnum },
    { rejectValue: string }
>(
    'orders/updateOrderStatus',
    async ({ orderNumber, newStatus }, { rejectWithValue }) => {
        try {
            const payload: UpdateOrderStatusCommand = { newStatus };
            const response = await api.apiV1OrdersOrderNumberStatusPatch(orderNumber, payload);
            return response.data;
        } catch (err) {
            const axiosErr = err as AxiosError<any>;
            if (axiosErr.response && axiosErr.response.status === 400) {
                const data = axiosErr.response.data as any;
                const msg = (data && (data.detail || data.title || data.message)) || 'Incorrect status';
                return rejectWithValue(String(msg));
            }
            throw err;
        }
    }
);

export const createOrder = createAsyncThunk(
  'orders/createOrder',
  async ({ description }: { description: string }) => {
    const response = await api.apiV1OrdersPost({ description });
    return response.data;
  }
);

// Sorting is performed server-side via orderBy param

const ordersSlice = createSlice({
    name: 'orders',
    initialState,
    reducers: {
        setQuery(state, action: PayloadAction<Partial<OrdersQuery>>) {
            state.lastQuery = { ...state.lastQuery, ...action.payload } as OrdersQuery;
        },
        clearSelected(state) {
            state.selected = undefined;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchOrders.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
                const query = action.meta.arg;
                state.lastQuery = { ...state.lastQuery, ...query } as OrdersQuery;
            })
            .addCase(fetchOrders.fulfilled, (state, action) => {
                state.isLoading = false;
                const { data, query } = action.payload;
                state.items = data;
                state.totalKnown = data.length < query.limit;
            })
            .addCase(fetchOrders.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message || 'Failed to load orders';
            })
            .addCase(fetchOrderByNumber.pending, (state) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(fetchOrderByNumber.fulfilled, (state, action) => {
                state.isLoading = false;
                state.selected = action.payload;
            })
            .addCase(fetchOrderByNumber.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message || 'Failed to load order';
            })
            .addCase(updateOrderStatus.pending, (state) => {
                state.updateError = undefined;
                state.isUpdating = true;
            })
            .addCase(updateOrderStatus.fulfilled, (state, action) => {
                state.selected = action.payload;
                const index = state.items.findIndex(o => o.orderNumber === action.payload.orderNumber);
                if (index >= 0) state.items[index] = action.payload;
                state.isUpdating = false;
            })
            .addCase(updateOrderStatus.rejected, (state, action) => {
                state.updateError = (action.payload as string) || action.error.message || 'Не удалось обновить статус';
                state.isUpdating = false;
            });
    },
});

export const { setQuery, clearSelected } = ordersSlice.actions;
export default ordersSlice.reducer;


