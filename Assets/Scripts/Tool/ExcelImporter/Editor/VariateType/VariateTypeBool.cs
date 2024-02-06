using System.Text;

namespace Engine.Excel
{
    class VariateTypeBool : VariateTypeBase
    {
        public override string TypeName => "bool";
        public override string CSTypeName => "Bool";

        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeBool>(name, columnIndex);
        }

        public override bool TryParse(string valueString, int rowIdx,ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out bool res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }

    class VariateTypeBoolArray : VariateTypeBool ,IVariateArray
    {
        public override string TypeName => "bool[]";
        public override string CSTypeName => "BoolAry";
        public override string BaseTypeName => base.TypeName;

        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeBoolArray>(name, columnIndex,';');
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