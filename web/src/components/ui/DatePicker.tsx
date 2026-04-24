import { type InputHTMLAttributes, forwardRef } from 'react'
import { cn } from '../../utils/cn'

interface DatePickerProps extends Omit<InputHTMLAttributes<HTMLInputElement>, 'type'> {
  label?: string
  error?: string
}

const DatePicker = forwardRef<HTMLInputElement, DatePickerProps>(
  ({ label, error, className, id, ...props }, ref) => {
    const inputId = id || label?.toLowerCase().replace(/\s+/g, '-')

    return (
      <div>
        {label && (
          <label htmlFor={inputId} className="block text-[13px] font-semibold text-slate-700 mb-2">
            {label}
          </label>
        )}
        <input
          ref={ref}
          id={inputId}
          type="date"
          className={cn(
            'w-full px-4 py-3 rounded-xl border outline-none transition-all duration-200 text-[14px] bg-slate-50/50 focus:bg-white',
            error
              ? 'border-red-300 focus:border-red-400 focus:ring-2 focus:ring-red-100'
              : 'border-slate-200 focus:border-orange-300 focus:ring-2 focus:ring-orange-100',
            props.disabled && 'opacity-50 cursor-not-allowed bg-slate-100',
            className,
          )}
          {...props}
        />
        {error && <p className="mt-1.5 text-[12px] text-red-500">{error}</p>}
      </div>
    )
  },
)

DatePicker.displayName = 'DatePicker'
export default DatePicker
