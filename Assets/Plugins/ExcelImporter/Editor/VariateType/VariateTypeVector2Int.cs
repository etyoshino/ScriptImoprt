using UnityEngine;

namespace Excel
{
    public class VariateTypeVector2Int : VariateTypeBase
    {
        public override string TypeName => "vector2";
        public override string FullTypeName => "v2Int";
     
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out Vector2Int result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }

    public class VariateTypeVector2IntArray : VariateTypeVector2Int , IVariateArray
    {
        public override string TypeName => "vector2[]";
        public override string FullTypeName => "v2Int[]";
        
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