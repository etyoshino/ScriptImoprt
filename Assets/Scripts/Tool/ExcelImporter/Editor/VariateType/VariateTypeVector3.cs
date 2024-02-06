using System.Text;
using UnityEngine;

namespace Engine.Excel
{
    class VariateTypeVector3 : VariateTypeBase
    {
        public override string FullTypeName => "Vector3";
        public override string TypeName => "V3";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3>(name, columnIndex);
        }

        public override bool TryParse(string valueString, int rowIdx, ref StringBuilder result)
        {
            if (!TryGetValue(valueString, out Vector3 res))
            {
                result.Clear();
                result.Append(ParseLogError(rowIdx));
                return false;
            }

            result.Append(res);
            return true;
        }
    }

    class VariateTypeVector3Array : VariateTypeVector3, IVariateArray
    {
        public override string FullTypeName => "Vector3[]";
        public override string TypeName => "V3[]";
        public override string CSTypeName => "Vector3Ary";
        public override string BaseTypeName => base.TypeName;
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3Array>(name, columnIndex,';');
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