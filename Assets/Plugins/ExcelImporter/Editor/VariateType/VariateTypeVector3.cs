using UnityEngine;

namespace Excel
{
    public class VariateTypeVector3 : VariateTypeBase
    {
        public override string FullTypeName => "Vector3";
        public override string TypeName => "V3";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3>(name, columnIndex);
        }

        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out Vector3 result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }

    public class VariateTypeVector3Array : VariateTypeVector3, IVariateArray
    {
        public override string FullTypeName => "Vector3[]";
        public override string TypeName => "V3[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeVector3Array>(name, columnIndex);
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