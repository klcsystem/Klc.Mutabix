import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import type { UseQueryOptions, UseMutationOptions } from '@tanstack/react-query'
import apiClient from '../api/client'
import type { AxiosError } from 'axios'

export function useApiQuery<T>(
  key: string[],
  url: string,
  options?: Omit<UseQueryOptions<T, AxiosError>, 'queryKey' | 'queryFn'>,
) {
  return useQuery<T, AxiosError>({
    queryKey: key,
    queryFn: async () => {
      const { data } = await apiClient.get<T>(url)
      return data
    },
    ...options,
  })
}

export function useApiMutation<TData, TVariables>(
  url: string,
  method: 'post' | 'put' | 'patch' | 'delete' = 'post',
  invalidateKeys?: string[][],
  options?: Omit<UseMutationOptions<TData, AxiosError, TVariables>, 'mutationFn'>,
) {
  const queryClient = useQueryClient()

  return useMutation<TData, AxiosError, TVariables>({
    mutationFn: async (variables) => {
      const { data } = await apiClient[method]<TData>(url, variables)
      return data
    },
    onSuccess: (...args) => {
      invalidateKeys?.forEach((key) => queryClient.invalidateQueries({ queryKey: key }))
      options?.onSuccess?.(...args)
    },
    ...options,
  })
}
