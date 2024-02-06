using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace Engine.Excel
{
    public interface IVariateArray
    {
        bool TryParseArrayElement(string valueString, int rowIdx, ref StringBuilder logError);
    }

    public interface IVariateArrayArray : IVariateArray
    {
        public char AryArySplitChar => '|';
    }

    [Flags]
    public enum EVariateAttribute : uint
    {
        None = 0,                // 不做处理
        Key = 1 << 1,            // 索引
        Union = 1 << 2,          // 分组
        Replace = 1 << 3,        // 参数替换ParamData 替换表
        ReplaceFormat = 1 << 4,  // 参数替换ParamData 替换表 带格式
        Ignore = 1 << 6,         // 忽视
    }
    
    abstract class VariateTypeBase
    {
        public abstract string TypeName { get; }
        public virtual string FullTypeName => TypeName;
        public virtual string CSTypeName => FullTypeName;

        public virtual string BaseTypeName => FullTypeName;
        
        public string SourceTypeName;

        public string VariateDesc;
        
        public string Name;
        
        public int ColumnIndex;

        public EVariateAttribute VariateAttribute;
        public virtual char ArrarySplitChar { get; protected set; }
        //是否托管数据 默认不是
        public virtual bool IsUnmanagedType => false;

        public abstract VariateTypeBase CreateInstance(string name, int columnIndex);

        public static T _CreateInstance<T>(string name, int columnIndex, char splitChar = '\0')
            where T : VariateTypeBase, new()
        {
            T result = new T();
            result.InitData(name, columnIndex,splitChar);
            return result;
        }

        public void InitData(string name, int columnIndex, char splitChar = '\0')
        {
            Name = name;
            ColumnIndex = columnIndex;
            ArrarySplitChar = splitChar;
            VariateAttribute = EVariateAttribute.None;
            VariateDesc = null;
        }

        public abstract bool TryParse(string valueString, int rowIdx, ref StringBuilder result);
        
        protected bool TryParseArray(IVariateArray arrayConfig, string valueString, int rowIndex, ref StringBuilder result)
        {
            return TryParseArray(arrayConfig, valueString, rowIndex, ref result, ArrarySplitChar);
        }

        protected bool TryParseArray(IVariateArray arrayConfig, string valueString, int rowIndex, ref StringBuilder result,
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
                if (!arrayConfig.TryParseArrayElement(values[i], rowIndex, ref result))
                {
                    return false;
                }
            }
            return true;
        }
        
        protected bool TryPaeseArrayArray(IVariateArrayArray arrayConfig,  string valueString, int rowIndex, ref StringBuilder result)
        {
            string[] values = valueString.Split('|');

            if (values.Length == 1 && string.IsNullOrEmpty(values[0]))
            {
                return true;
            }
            
            for (int i = 0, iLength = values.Length; i < iLength; i++)
            {
                if (!TryParseArray(arrayConfig,  values[i], rowIndex, ref result))
                {
                    return false;
                }
            }
            return true;
        }
        
        protected string ParseLogError(int rowIndex)
        {
            return ParseLogError(rowIndex, ColumnIndex, Name, TypeName);
        }
        internal static string ParseLogError(int rowIndex, int columnIndex, string name, string typeName)
        {
            StackFrame stackFrame = new StackFrame(true);
            string callerName = stackFrame.GetMethod().Name;
            string callerLineNumber = stackFrame.GetFileLineNumber().ToString();
            string callerFilePath = stackFrame.GetFileName();
            string addLog = $"{Path.GetFileName(callerFilePath)} Function:{callerName} Line:{callerLineNumber}";

            return $"Excel Config Error: TryParse Error\nRow: {rowIndex + 1}, Column: {columnIndex + 1}, Name: {name}, Type: {typeName}\n{addLog}\n";
        }

        public void UpdateVariateAttribute(string attribute)
        {
            var isAry = this is IVariateArray;
            
            VariateAttribute |= attribute.ToUpper() switch
            {
                "KEY" => isAry ? EVariateAttribute.None : EVariateAttribute.Key,
                "UNION" => isAry ? EVariateAttribute.None : EVariateAttribute.Union,
                "IGNORE" => EVariateAttribute.Ignore,
                "REPLACE" => EVariateAttribute.Replace,
                "REPLACE1" => EVariateAttribute.ReplaceFormat,
                _ => EVariateAttribute.None,
            };
        }
        
        // sbyte parse
        public static bool TryGetValue(string valueString, out sbyte result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = sbyte.TryParse(valueString, out result);
            result = ok ? result : default(sbyte);
            return ok;
        }
        
        // short parse
        public static bool TryGetValue(string valueString, out short result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = short.TryParse(valueString, out result);
            result = ok ? result : default(short);
            return ok;
        }
        
        // int parse
        public static bool TryGetValue(string valueString, out int result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = int.TryParse(valueString, out result);
            result = ok ? result : default(int);
            return ok;
        }
        
        // long parse
        public static bool TryGetValue(string valueString, out long result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = long.TryParse(valueString, out result);
            result = ok ? result : default(long);
            return ok;
        }
        
        // byte parse
        public static bool TryGetValue(string valueString, out byte result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = byte.TryParse(valueString, out result);
            result = ok ? result : default(byte);
            return ok;
        }
        
        // ushort parse
        public static bool TryGetValue(string valueString, out ushort result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = ushort.TryParse(valueString, out result);
            result = ok ? result : default(ushort);
            return ok;
        }
        
        // uint parse
        public static bool TryGetValue(string valueString, out uint result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = uint.TryParse(valueString, out result);
            result = ok ? result : default(uint);
            return ok;
        }
        
        // ulong parse
        public static bool TryGetValue(string valueString, out ulong result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = ulong.TryParse(valueString, out result);
            result = ok ? result : default(ulong);
            return ok;
        }
        
        // float parse
        public static bool TryGetValue(string valueString, out float result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = float.TryParse(valueString, out result);
            result = ok ? result : default(float);
            return ok;
        }
        
        // double parse
        public static bool TryGetValue(string valueString, out double result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
            var ok = double.TryParse(valueString, out result);
            result = ok ? result : default(double);
            return ok;
        }
        
        // bool parse
        public bool TryGetValue(string valueString, out bool result)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                result = default;
                return true;
            }
            
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