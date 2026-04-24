import { type SelectHTMLAttributes, forwardRef } from 'react'
import { cn } from '../../utils/cn'

interface SelectOption {
  value: string
  label: string
}

interface SelectProps extends SelectHTMLAttributes<HTMLSelectElement> {
  label?: string
  error?: string
  options: SelectOption[]
  placeholder?: string
}

const Select = forwardRef<HTMLSelectElement, SelectProps>(
  ({ label, error, options, placeholder, className, id, ...props }, ref) => {
    const selectId = id || label?.toLowerCase().replace(/\s+/g, '-')

    return (
      <div>
        {label && (
          <label htmlFor={selectId} className="block text-[13px] font-semibold text-slate-700 mb-2">
            {label}
          </label>
        )}
        <select
          ref={ref}
          id={selectId}
          className={cn(
            'w-full px-4 py-3 rounded-xl border outline-none transition-all duration-200 text-[14px] bg-slate-50/50 focus:bg-white appearance-none',
            error
              ? 'border-red-300 focus:border-red-400 focus:ring-2 focus:ring-red-100'
              : 'border-slate-200 focus:border-orange-300 focus:ring-2 focus:ring-orange-100',
            props.disabled && 'opacity-50 cursor-not-allowed bg-slate-100',
            className,
          )}
          {...props}
        >
          {placeholder && <option value="">{placeholder}</option>}
          {options.map((opt) => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
          ))}
        </select>
        {error && <p className="mt-1.5 text-[12px] text-red-500">{error}</p>}
      </div>
    )
  },
)

Select.displayName = 'Select'
export default Select
