using System.Text;
using UnityEngine;

namespace Engine.Excel
{
    class VariateTypeVector2Int : VariateTypeBase
    {
        public override string FullTypeName => "Vector2Int";
        public override string TypeName => "V2Int";
     
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector2Int>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out Vector2Int res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }

    class VariateTypeVector2IntArray : VariateTypeVector2Int , IVariateArray
    {
        public override string FullTypeName => "Vector2Int[]";
        public override string TypeName => "V2Int[]";
        public override string CSTypeName => "Vector2IntAry";
        public override string BaseTypeName => base.TypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector2IntArray>(name, columnIndex,';');
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