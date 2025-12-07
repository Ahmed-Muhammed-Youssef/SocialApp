export interface PagedList<T> {
  items: T[];
  pageNumber: number;
  itemsPerPage: number;
  totalCount: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}