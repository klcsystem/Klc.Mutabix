import { useState, useRef, type DragEvent } from 'react'
import { Upload, File, X } from 'lucide-react'
import { cn } from '../../utils/cn'

interface FileUploadProps {
  label?: string
  accept?: string
  onFileSelect: (file: File) => void
  onClear?: () => void
  className?: string
}

export default function FileUpload({ label, accept, onFileSelect, onClear, className }: FileUploadProps) {
  const [fileName, setFileName] = useState<string | null>(null)
  const [isDragging, setIsDragging] = useState(false)
  const inputRef = useRef<HTMLInputElement>(null)

  const handleFile = (file: File) => {
    setFileName(file.name)
    onFileSelect(file)
  }

  const handleDrop = (e: DragEvent) => {
    e.preventDefault()
    setIsDragging(false)
    const file = e.dataTransfer.files[0]
    if (file) handleFile(file)
  }

  const handleClear = () => {
    setFileName(null)
    if (inputRef.current) inputRef.current.value = ''
    onClear?.()
  }

  return (
    <div>
      {label && <p className="block text-[13px] font-semibold text-slate-700 mb-2">{label}</p>}

      {fileName ? (
        <div className="flex items-center gap-3 px-4 py-3 rounded-xl border border-slate-200 bg-slate-50/50">
          <File className="w-4 h-4 text-orange-500" />
          <span className="flex-1 text-[13px] text-slate-700 truncate">{fileName}</span>
          <button onClick={handleClear} className="p-1 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors">
            <X className="w-4 h-4" />
          </button>
        </div>
      ) : (
        <div
          onDragOver={(e) => { e.preventDefault(); setIsDragging(true) }}
          onDragLeave={() => setIsDragging(false)}
          onDrop={handleDrop}
          onClick={() => inputRef.current?.click()}
          className={cn(
            'flex flex-col items-center gap-2 px-6 py-8 rounded-xl border-2 border-dashed cursor-pointer transition-all duration-200',
            isDragging
              ? 'border-orange-400 bg-orange-50/50'
              : 'border-slate-200 hover:border-orange-300 hover:bg-slate-50',
            className,
          )}
        >
          <Upload className="w-6 h-6 text-slate-400" />
          <p className="text-[13px] text-slate-500">Dosya surukleyin veya tiklayin</p>
          <input
            ref={inputRef}
            type="file"
            accept={accept}
            onChange={(e) => { const f = e.target.files?.[0]; if (f) handleFile(f) }}
            className="hidden"
          />
        </div>
      )}
    </div>
  )
}
