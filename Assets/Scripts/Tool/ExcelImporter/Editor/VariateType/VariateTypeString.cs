using System.Text;

namespace Engine.Excel
{
    class VariateTypeString : VariateTypeBase
    {
        public override string TypeName => "string";
        public override string CSTypeName => "String";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeString>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            return true;
        }
    }

    class VariateTypeStringArray : VariateTypeString, IVariateArray
    {
        public override string TypeName => "string[]";
        public override string CSTypeName => "StringAry";
        public override string BaseTypeName => base.TypeName;

        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeStringArray>(name, columnIndex,';');
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