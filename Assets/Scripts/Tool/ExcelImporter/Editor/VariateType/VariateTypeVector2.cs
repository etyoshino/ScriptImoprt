using System.Text;
using UnityEngine;

namespace Engine.Excel
{
    class VariateTypeVector2 : VariateTypeBase
    {
        public override string FullTypeName => "Vector2";
        public override string TypeName => "V2";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector2>(name, columnIndex);
        }

        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out Vector2 res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }

    class VariateTypeVector2Array : VariateTypeVector2, IVariateArray
    {
        public override string FullTypeName => "Vector2[]";
        public override string TypeName => "V2[]";
        public override string CSTypeName => "Vector2Ary";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector2Array>(name, columnIndex,';');
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