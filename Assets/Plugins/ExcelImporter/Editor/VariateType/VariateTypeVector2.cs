using UnityEngine;

namespace Excel
{
    public class VariateTypeVector2 : VariateTypeBase
    {
        public override string FullTypeName => "Vector2";
        public override string TypeName => "V2";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector2>(name, columnIndex);
        }

        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!TryGetValue(valueString, out Vector2 result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }

    public class VariateTypeVector2Array : VariateTypeVector2, IVariateArray
    {
        public override string FullTypeName => "Vector2[]";
        public override string TypeName => "V2[]";

        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector2Array>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            return TryPraseArrayElement(valueString, rowIdx, ref logError);
        }

        public bool TryPraseArrayElement(string valueString, int rowIdx, ref string logError)
        {
            return base.TryPrase(valueString, rowIdx, ref logError);
        }
    }
}