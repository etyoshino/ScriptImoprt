using UnityEngine;

namespace Excel
{
    public class VariateTypeVector3Int : VariateTypeBase
    {
        public override string FullTypeName => "Vector3Int";
        public override string TypeName => "V3Int";
     
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3Int>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!TryGetValue(valueString, out Vector2Int result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }

    public class VariateTypeVector3IntArray : VariateTypeVector3Int , IVariateArray
    {
        public override string FullTypeName => "Vector3Int[]";
        public override string TypeName => "V3Int[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3IntArray>(name, columnIndex);
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