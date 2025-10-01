export interface ApiError {
  detail?: string;
  title?: string;
  message?: string;
}

export interface PaginationParams {
  limit: number;
  offset: number;
}

export interface OrderQuery extends PaginationParams {
  freeText?: string;
  orderStatus?: number[];
  orderBy?: string;
}
