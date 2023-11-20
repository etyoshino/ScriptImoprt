using System.Diagnostics;
using System.IO;

namespace Excel
{
    public interface IVariateArray
    {
        bool TryPraseArrayElement(string valueString, int rowIdx, ref string logError);
    }
    
    public abstract class VariateTypeBase
    {
        public abstract string TypeName { get; }
        public virtual string FullTypeName => TypeName;
        
        public string Name;
        
        public int ColumnIndex;
        
        protected const char SplitChar = '.';
        
        protected const char ArrarySplitChar = '|';
        
        public static T CreateInstance<T>(string name, int columnIndex)
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
            return PraseLogError(rowIndex, ColumnIndex, Name, TypeName, 2);
        }
        internal static string PraseLogError(int rowIndex, int columnIndex, string name, string typeName, int skipFrames)
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            string callerName = st.GetFrame(0).GetMethod().Name;
            string callerLineNumber = st.GetFrame(0).GetFileLineNumber().ToString();
            string callerFilePath = st.GetFrame(0).GetFileName();
            string addLog = $"\n{Path.GetFileName(callerFilePath)} Function:{callerName} Line:{callerLineNumber}";

            return $"Excel Config Error: TryPrase Error\nRow: {rowIndex + 1}, Column: {columnIndex + 1}, Name: {name}, Type: {typeName}\n{addLog}\n";
        }
    }
}