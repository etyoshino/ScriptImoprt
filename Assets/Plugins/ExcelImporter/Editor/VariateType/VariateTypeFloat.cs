namespace Excel
{
    public class VariateTypeFloat : VariateTypeBase
    {
        public override string TypeName => "float";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeFloat>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out float result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeFloatArray : VariateTypeFloat , IVariateArray
    {
        public override string TypeName => "float[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeFloatArray>(name, columnIndex);
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