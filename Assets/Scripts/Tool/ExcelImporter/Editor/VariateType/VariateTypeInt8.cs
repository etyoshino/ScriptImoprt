using System.Text;

namespace Engine.Excel
{
    class VariateTypeInt8 : VariateTypeBase
    {
        public override string TypeName => "Int8";
        public override string FullTypeName => "sbyte";
        public override string CSTypeName => "SByte";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt8>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out sbyte res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeInt8Array : VariateTypeInt8 , IVariateArray
    {
        public override string TypeName => "Int8[]";
        public override string FullTypeName => "sbyte[]";
        public override string CSTypeName => "SByteAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt8Array>(name, columnIndex,';');
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
    
    class VariateTypeInt8ArrayArray : VariateTypeInt8Array , IVariateArrayArray
    {
        public override string TypeName => "Int8[][]";
        public override string FullTypeName => "sbyte[][]";
        public override string CSTypeName => "SByteAryAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt8ArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
    
    class VariateTypeUInt8 : VariateTypeBase
    {
        public override string TypeName => "UInt8";
        public override string FullTypeName => "byte";
        public override string CSTypeName => "Byte";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt8>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out byte res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeUInt8Array : VariateTypeUInt8 , IVariateArray
    {
        public override string TypeName => "UInt8[]";
        public override string FullTypeName => "byte[]";
        public override string CSTypeName => "ByteAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt8Array>(name, columnIndex,';');
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
    
    class VariateTypeUInt8ArrayArray : VariateTypeUInt8Array , IVariateArrayArray
    {
        public override string TypeName => "UInt8[][]";
        public override string FullTypeName => "byte[][]";
        public override string CSTypeName => "ByteAryAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt8ArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
}