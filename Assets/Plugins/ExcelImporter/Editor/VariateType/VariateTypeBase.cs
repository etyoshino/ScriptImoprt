using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Excel
{
    public interface IVariateArray
    {
        bool TryPraseArrayElement(string valueString, int rowIdx, ref string logError);
    }

    [Flags]
    public enum EVariateAttribute : uint
    {
        None = 0,                // 不做处理
        Key = 1 << 1,            // 索引
        Union = 1 << 2,          // 分组
        Replace = 1 << 3,        // 参数替换ParamData 替换表
        Ignore = 1 << 4,         // 忽视
    }
    
    public abstract class VariateTypeBase
    {
        public abstract string TypeName { get; }
        public virtual string FullTypeName => TypeName;

        public string VariateDesc = "";
        
        public string Name;
        
        public int ColumnIndex;

        public EVariateAttribute VariateAttribute;
        
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
            VariateAttribute = EVariateAttribute.None;
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
            VariateAttribute |= attribute.ToUpper() switch
            {
                "KEY" => EVariateAttribute.Key,
                "UNION" => EVariateAttribute.Union,
                "IGNORE" => EVariateAttribute.Ignore,
                "REPLACE" => EVariateAttribute.Replace,
                _ => EVariateAttribute.None,
            };
        }
        
        // sbyte parse
        public bool TryGetValue(string valueString, out sbyte result)
        {
            var ok = sbyte.TryParse(valueString, out result);
            result = ok ? result : default(sbyte);
            return ok;
        }
        
        // short parse
        public bool TryGetValue(string valueString, out short result)
        {
            var ok = short.TryParse(valueString, out result);
            result = ok ? result : default(short);
            return ok;
        }
        
        // int parse
        public bool TryGetValue(string valueString, out int result)
        {
            var ok = int.TryParse(valueString, out result);
            result = ok ? result : default(int);
            return ok;
        }
        
        // long parse
        public bool TryGetValue(string valueString, out long result)
        {
            var ok = long.TryParse(valueString, out result);
            result = ok ? result : default(long);
            return ok;
        }
        
        // byte parse
        public bool TryGetValue(string valueString, out byte result)
        {
            var ok = byte.TryParse(valueString, out result);
            result = ok ? result : default(byte);
            return ok;
        }
        
        // ushort parse
        public bool TryGetValue(string valueString, out ushort result)
        {
            var ok = ushort.TryParse(valueString, out result);
            result = ok ? result : default(ushort);
            return ok;
        }
        
        // uint parse
        public bool TryGetValue(string valueString, out uint result)
        {
            var ok = uint.TryParse(valueString, out result);
            result = ok ? result : default(uint);
            return ok;
        }
        
        // ulong parse
        public bool TryGetValue(string valueString, out ulong result)
        {
            var ok = ulong.TryParse(valueString, out result);
            result = ok ? result : default(ulong);
            return ok;
        }
        
        // float parse
        public bool TryGetValue(string valueString, out float result)
        {
            var ok = float.TryParse(valueString, out result);
            result = ok ? result : default(float);
            return ok;
        }
        
        // double parse
        public bool TryGetValue(string valueString, out double result)
        {
            var ok = double.TryParse(valueString, out result);
            result = ok ? result : default(double);
            return ok;
        }
        
        // string parse
        public bool TryGetValue(string valueString, out string result)
        {
            result = valueString;
            return true;
        }
        
        // bool parse
        public bool TryGetValue(string valueString, out bool result)
        {
            var ok = bool.TryParse(valueString, out result);
            result = ok ? result : default(bool);
            return ok;
        }

        #region Vector

        // Vector2 parse
        public bool TryGetValue(string valueString, out Vector2 result,char splitChar = '_')
        {
            result = Vector2.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 2)
            {
                return false;
            }

            if (!float.TryParse(variates[0], out float x))
            {
                return false;
            }
            if (!float.TryParse(variates[1], out float y))
            {
                return false;
            }
            
            result.Set(x,y);
            return true;
        }
        
        // Vector2Int parse
        public bool TryGetValue(string valueString, out Vector2Int result,char splitChar = '_')
        {
            result = Vector2Int.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(variates[0], out int x))
            {
                return false;
            }
            if (!int.TryParse(variates[1], out int y))
            {
                return false;
            }
            
            result.Set(x,y);
            return true;
        }
        
        // Vector3 parse
        public bool TryGetValue(string valueString, out Vector3 result,char splitChar = '_')
        {
            result = Vector3.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 3)
            {
                return false;
            }

            if (!float.TryParse(variates[0], out float x))
            {
                return false;
            }
            if (!float.TryParse(variates[1], out float y))
            {
                return false;
            }
            if (!float.TryParse(variates[2], out float z))
            {
                return false;
            }


            result.Set(x, y, z);
            return true;
        }
        
        // Vector3Int parse
        public bool TryGetValue(string valueString, out Vector3Int result,char splitChar = '_')
        {
            result = Vector3Int.zero;
            if (string.IsNullOrEmpty(valueString))
            {
                return true;
            }

            var variates = valueString.Split(splitChar);
            if (variates.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(variates[0], out int x))
            {
                return false;
            }
            if (!int.TryParse(variates[1], out int y))
            {
                return false;
            }
            if (!int.TryParse(variates[2], out int z))
            {
                return false;
            }


            result.Set(x, y, z);
            return true;
        }
        #endregion
    }
}