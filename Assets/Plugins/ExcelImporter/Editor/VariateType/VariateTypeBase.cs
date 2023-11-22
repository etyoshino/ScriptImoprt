using System;
using System.Diagnostics;
using System.IO;

namespace Excel
{
    public interface IVariateArray
    {
        bool TryPraseArrayElement(string valueString, int rowIdx, ref string logError);
    }

    [Flags]
    public enum VariateAttribute : uint
    {
        None = 0,               // 不做处理
        Key = 1 << 1,           // 索引
        Union = 1 << 2,          // 分组
        Replace = 1 << 4,    // 参数替换ParamData 替换表
        Ignore = 1 << 8,     // 忽视
    }
    
    public abstract class VariateTypeBase
    {
        public abstract string TypeName { get; }
        public virtual string FullTypeName => TypeName;

        public string VariateDesc = "";
        
        public string Name;
        
        public int ColumnIndex;

        public VariateAttribute VariateAttribute;
        
        protected const char SplitChar = ',';
        
        protected const char ArrarySplitChar = '|';
        
        public abstract VariateTypeBase CreateInstance(string name, int columnIndex);
        
        public static T _CreateInstance<T>(string name, int columnIndex)
            where T : VariateTypeBase, new()
        {
            T result = new T();
            result.InitData(name, columnIndex);
            return result;
        }
        
        public void InitData(string name,int columnIndex)
        {
            Name = name;
            ColumnIndex = columnIndex;
            VariateAttribute = VariateAttribute.None;
        }

        public abstract bool TryPrase(string valueString, int rowIdx, ref string logError);
        
        protected bool TryPraseArray(IVariateArray arrayConfig, string valueString, int rowIndex, ref string logError)
        {
            return TryPraseArray(arrayConfig, valueString, rowIndex, ref logError, ArrarySplitChar);
        }

        protected bool TryPraseArray(IVariateArray arrayConfig, string valueString, int rowIndex, ref string logError,
            char arraySplitChar)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }
            
            string[] values = valueString.Split(arraySplitChar);

            if (values.Length == 1 && string.IsNullOrEmpty(values[0]))
            {
                return true;
            }
            
            for (int i = 0, iLength = values.Length; i < iLength; i++)
            {
                if (!arrayConfig.TryPraseArrayElement(values[i], rowIndex, ref logError))
                {
                    return false;
                }
            }
            return true;
        }
        
        protected string PraseLogError(int rowIndex)
        {
            return PraseLogError(rowIndex, ColumnIndex, Name, TypeName);
        }
        internal static string PraseLogError(int rowIndex, int columnIndex, string name, string typeName)
        {
            StackFrame stackFrame = new StackFrame(true);
            string callerName = stackFrame.GetMethod().Name;
            string callerLineNumber = stackFrame.GetFileLineNumber().ToString();
            string callerFilePath = stackFrame.GetFileName();
            string addLog = $"{Path.GetFileName(callerFilePath)} Function:{callerName} Line:{callerLineNumber}";

            return $"Excel Config Error: TryPrase Error\nRow: {rowIndex + 1}, Column: {columnIndex + 1}, Name: {name}, Type: {typeName}\n{addLog}\n";
        }

        public void UpdateVariateAttribute(string attribute)
        {
            VariateAttribute &= attribute switch
            {
                "KEY" => VariateAttribute.Key,
                "UNION" => VariateAttribute.Union,
                "IGNORE" => VariateAttribute.Ignore,
                "REPLACE" => VariateAttribute.Replace,
                _ => VariateAttribute.None,
            };
        }
    }
}