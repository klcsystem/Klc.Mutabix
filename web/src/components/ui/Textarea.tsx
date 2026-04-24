import { type TextareaHTMLAttributes, forwardRef } from 'react'
import { cn } from '../../utils/cn'

interface TextareaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  label?: string
  error?: string
}

const Textarea = forwardRef<HTMLTextAreaElement, TextareaProps>(
  ({ label, error, className, id, rows = 4, ...props }, ref) => {
    const textareaId = id || label?.toLowerCase().replace(/\s+/g, '-')

    return (
      <div>
        {label && (
          <label htmlFor={textareaId} className="block text-[13px] font-semibold text-slate-700 mb-2">
            {label}
          </label>
        )}
        <textarea
          ref={ref}
          id={textareaId}
          rows={rows}
          className={cn(
            'w-full px-4 py-3 rounded-xl border outline-none transition-all duration-200 text-[14px] bg-slate-50/50 focus:bg-white placeholder:text-slate-400 resize-none',
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

Textarea.displayName = 'Textarea'
export default Textarea
