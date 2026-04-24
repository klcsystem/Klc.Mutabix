import { type InputHTMLAttributes, type ReactNode, forwardRef } from 'react'
import { cn } from '../../utils/cn'

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string
  error?: string
  iconLeft?: ReactNode
  iconRight?: ReactNode
}

const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, iconLeft, iconRight, className, id, ...props }, ref) => {
    const inputId = id || label?.toLowerCase().replace(/\s+/g, '-')

    return (
      <div>
        {label && (
          <label htmlFor={inputId} className="block text-[13px] font-semibold text-slate-700 mb-2">
            {label}
          </label>
        )}
        <div className="relative">
          {iconLeft && (
            <div className="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400">{iconLeft}</div>
          )}
          <input
            ref={ref}
            id={inputId}
            className={cn(
              'w-full py-3 rounded-xl border outline-none transition-all duration-200 text-[14px] bg-slate-50/50 focus:bg-white placeholder:text-slate-400',
              iconLeft ? 'pl-11 pr-4' : iconRight ? 'pl-4 pr-11' : 'px-4',
              error
                ? 'border-red-300 focus:border-red-400 focus:ring-2 focus:ring-red-100'
                : 'border-slate-200 focus:border-orange-300 focus:ring-2 focus:ring-orange-100',
              props.disabled && 'opacity-50 cursor-not-allowed bg-slate-100',
              className,
            )}
            {...props}
          />
          {iconRight && (
            <div className="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400">{iconRight}</div>
          )}
        </div>
        {error && <p className="mt-1.5 text-[12px] text-red-500">{error}</p>}
      </div>
    )
  },
)

Input.displayName = 'Input'
export default Input
