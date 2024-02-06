using System.Text;
using UnityEngine;

namespace Engine.Excel
{
    class VariateTypeVector3Int : VariateTypeBase
    {
        public override string FullTypeName => "Vector3Int";
        public override string TypeName => "V3Int";
     
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3Int>(name, columnIndex);
        }
        
        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out Vector3Int res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }

    class VariateTypeVector3IntArray : VariateTypeVector3Int , IVariateArray
    {
        public override string FullTypeName => "Vector3Int[]";
        public override string TypeName => "V3Int[]";
        public override string CSTypeName => "Vector3IntAry";
        public override string BaseTypeName => base.CSTypeName;
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3IntArray>(name, columnIndex,';');
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