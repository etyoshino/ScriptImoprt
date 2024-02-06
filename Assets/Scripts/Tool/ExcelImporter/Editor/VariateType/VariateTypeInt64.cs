using System.Text;

namespace Engine.Excel
{
    class VariateTypeInt64 : VariateTypeBase
    {
        public override string TypeName => "Int64";
        public override string FullTypeName => "long";
        public override string CSTypeName => "Long";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt64>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out long res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeInt64Array : VariateTypeInt64 , IVariateArray
    {
        public override string TypeName => "Int64[]";
        public override string FullTypeName => "long[]";
        public override string CSTypeName => "LongAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt64Array>(name, columnIndex,';');
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
    
    class VariateTypeUInt64 : VariateTypeBase
    {
        public override string TypeName => "UInt64";
        public override string FullTypeName => "ulong";
        public override string CSTypeName => "ULong";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt64>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out ulong res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeUInt64Array : VariateTypeUInt64 , IVariateArray
    {
        public override string TypeName => "UInt64[]";
        public override string FullTypeName => "ulong[]";
        public override string CSTypeName => "ULongAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt64Array>(name, columnIndex,';');
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