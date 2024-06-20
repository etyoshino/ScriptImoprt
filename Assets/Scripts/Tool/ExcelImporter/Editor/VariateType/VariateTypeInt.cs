using System.Text;

namespace Engine.Excel
{
    class VariateTypeInt : VariateTypeBase
    {
        public override string TypeName => "Int32";
        public override string FullTypeName => "int";
        public override string CSTypeName => "Int";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out int res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeIntArray : VariateTypeInt , IVariateArray
    {
        public override string TypeName => "Int32[]";
        public override string FullTypeName => "int[]";
        public override string CSTypeName => "IntAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeIntArray>(name, columnIndex,';');
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
    
    class VariateTypeIntArrayArray : VariateTypeIntArray , IVariateArrayArray
    {
        public override string TypeName => "Int32[][]";
        public override string FullTypeName => "int[][]";
        public override string CSTypeName => "IntAryAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeIntArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
    
    class VariateTypeUInt : VariateTypeBase
    {
        public override string TypeName => "UInt32";
        public override string FullTypeName => "uint";
        public override string CSTypeName => "UInt";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out uint res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeUIntArray : VariateTypeUInt , IVariateArray
    {
        public override string TypeName => "Uint32[]";
        public override string FullTypeName => "uint[]";
        public override string CSTypeName => "UIntAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUIntArray>(name, columnIndex,';');
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
    
    class VariateTypeUIntArrayArray : VariateTypeUIntArray , IVariateArrayArray
    {
        public override string TypeName => "Uint32[][]";
        public override string FullTypeName => "uint[][]";
        public override string CSTypeName => "UIntAryAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUIntArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
}