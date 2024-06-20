using System.Text;

namespace Engine.Excel
{
    class VariateTypeInt16 : VariateTypeBase
    {
        public override string TypeName => "Int16";
        public override string FullTypeName => "short";
        public override string CSTypeName => "Short";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt16>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out short res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeInt16Array : VariateTypeInt16 , IVariateArray
    {
        public override string TypeName => "Int16[]";
        public override string FullTypeName => "short[]";
        public override string CSTypeName => "ShortAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt16Array>(name, columnIndex,';');
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
    
    class VariateTypeInt16ArrayArray : VariateTypeInt16Array , IVariateArrayArray
    {
        public override string TypeName => "Int16[][]";
        public override string FullTypeName => "short[][]";
        public override string CSTypeName => "ShortAryAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt16ArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
    
    class VariateTypeUInt16 : VariateTypeBase
    {
        public override string TypeName => "UInt16";
        public override string FullTypeName => "ushort";
        public override string CSTypeName => "UShort";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt16>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out ushort res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeUInt16Array : VariateTypeUInt16 , IVariateArray
    {
        public override string TypeName => "UInt16[]";
        public override string FullTypeName => "ushort[]";
        public override string CSTypeName => "UShortAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt16Array>(name, columnIndex,';');
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
    
    class VariateTypeUInt16ArrayArray : VariateTypeUInt16Array , IVariateArrayArray
    {
        public override string TypeName => "UInt16[][]";
        public override string FullTypeName => "ushort[][]";
        public override string CSTypeName => "UShortAryAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt16ArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
}