using System.Text;

namespace Engine.Excel
{
    class VariateTypeDouble : VariateTypeBase
    {
        public override string TypeName => "double";
        public override string CSTypeName => "Double";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeDouble>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out double res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }
    
    class VariateTypeDoubleArray : VariateTypeDouble , IVariateArray
    {
        public override string TypeName => "double[]";
        public override string CSTypeName => "DoubleAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeDoubleArray>(name, columnIndex,';');
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
    
    class VariateTypeDoubleArrayArray : VariateTypeDoubleArray , IVariateArrayArray
    {
        public override string TypeName => "double[][]";
        public override string CSTypeName => "DoubleAryAry";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeDoubleArrayArray>(name, columnIndex,';');
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return TryParseArrayArray(this, valueString, rowIdx, ref result);
        }
    }
}