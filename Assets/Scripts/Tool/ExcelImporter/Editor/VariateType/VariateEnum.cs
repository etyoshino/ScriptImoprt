using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Engine.Excel
{
    abstract class VariateEnumBase : VariateTypeBase
    {
        public override bool IsUnmanagedType => true;
        protected string _typeName;
        public override string TypeName => _typeName;

        public string EnumName;
        
        public HashSet<string> EnumKeys;
        public virtual bool CreateType => false;
        
        protected static Regex _regex = new Regex(@"<\b\w*\b>");

        public virtual void InitEnumType(string name)
        {
            EnumName = name;
            _typeName = EnumName;
        }

        public static bool EnumCheck(string name, string typename, int columnIndex, out VariateTypeBase variateEnum)
        {
            variateEnum = null;
            if (name.Contains("CustomEnum", StringComparison.OrdinalIgnoreCase))
            {
                var match = _regex.Match(name);
                if (match.Success)
                {
                    var value = match.Value[1..^1];
                    var type = GetVariateType(name);
                    var tmpVariateEnum = type switch
                    {
                        EVariateType.Single => _CreateInstance<VariateCustEnum>(typename, columnIndex),
                        EVariateType.Array => _CreateInstance<VariateCustEnumArray>(typename, columnIndex, ';'),
                    };
                    tmpVariateEnum.InitEnumType(value);
                    variateEnum = tmpVariateEnum;
                    return true;
                }
            }
            else if (name.Contains("Enum", StringComparison.OrdinalIgnoreCase))
            {
                var match = _regex.Match(name);
                if (match.Success)
                {
                    var value = match.Value[1..^1];
                    var type = GetVariateType(name);
                    var tmpVariateEnum = type switch
                    {
                        EVariateType.Single => _CreateInstance<VariateEnum>(typename, columnIndex),
                        EVariateType.Array => _CreateInstance<VariateEnumArray>(typename, columnIndex, ';'),
                    };
                    tmpVariateEnum.InitEnumType(value);
                    variateEnum = tmpVariateEnum;
                    return true;
                }
            }
            
            return false;
        }

        private static EVariateType GetVariateType(string variateName)
        {
            var count = 0;
            var idx = variateName.IndexOf("[]");
            while (idx != -1)
            {
                count++;
                idx = variateName.IndexOf("[]", idx + 1);
            }

            if (count == 0)
            {
                idx = variateName.IndexOf("Ary");
                while (idx != -1)
                {
                    count++;
                    idx = variateName.IndexOf("Ary", idx + 1);
                }
            }

            return count switch
            {
                1 => EVariateType.Array,
                2 => EVariateType.ArrayArray,
                _ => EVariateType.Single,
            };
        }
    }
    
    class VariateCustEnum : VariateEnumBase
    {
        public override string CSTypeName => "Enum";
        protected Type enumType;
        public override void InitEnumType(string name)
        {
            EnumName = name;
            _typeName = EnumName;
            ExcelCommonField.EnumDic.TryGetValue(name, out enumType);
        }
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateEnum>(name, columnIndex);
        }
    
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (enumType is null)
            {
                return false;
            }
            
            if (string.IsNullOrEmpty(valueString))
            {
                result.Append(enumType.GetEnumValues().GetValue(0));
                return true;
            }
    
            if(int.TryParse(valueString,out var value))
            {
                if (Enum.IsDefined(enumType, value))
                {
                    result.Append(valueString);
                    return true;
                }
            }
            
            if (!Enum.IsDefined(enumType, valueString))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }
    
            result.Append(valueString);
            return true;
        }
    }
    
    class VariateCustEnumArray : VariateCustEnum ,IVariateArray
    {
        public override string CSTypeName => "EnumAry";
        public override string BaseTypeName => EnumName;
        public override void InitEnumType(string name)
        {
            EnumName = name;
            _typeName = name + "[]";
            ExcelCommonField.EnumDic.TryGetValue(name,out enumType);
        }
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateEnumArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArray(this, valueString, rowIdx, ref result);
        }
    
        public bool TryParseArrayElement(string valueString, int rowIdx, ref StringBuilder logError)
        {
            return base.TryParse(valueString, rowIdx, ref logError);
        }
    }
    
    class VariateEnum : VariateEnumBase
    {
        public override bool CreateType => true;
        public override string CSTypeName => "Enum";
        
        public override void InitEnumType(string name)
        {
            EnumName = name;
            _typeName = EnumName;
            EnumKeys = new HashSet<string>();
        }
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateEnum>(name, columnIndex);
        }

        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (int.TryParse(valueString, out _))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }
            
            if(!string.IsNullOrEmpty(valueString)) EnumKeys.Add(valueString);
            result.Append(valueString);
            return true;
        }
    }
    
    class VariateEnumArray : VariateEnum ,IVariateArray
    {
        public override bool CreateType => true;
        public override string CSTypeName => "EnumAry";
        public override string BaseTypeName => EnumName;
        public override void InitEnumType(string name)
        {
            EnumName = name;
            _typeName = name + "[]";
            EnumKeys = new HashSet<string>();
        }
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateEnumArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArray(this, valueString, rowIdx, ref result);
        }

        public bool TryParseArrayElement(string valueString, int rowIdx, ref StringBuilder logError)
        {
            return base.TryParse(valueString, rowIdx, ref logError);
        }
    }
}