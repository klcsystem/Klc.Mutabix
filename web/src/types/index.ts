export interface PaginatedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
}

export interface ApiError {
  message: string
  errors?: Record<string, string[]>
}

export interface SelectOption {
  value: string
  label: string
}
