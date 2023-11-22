namespace Excel
{
    public class VariateTypeDouble : VariateTypeBase
    {
        public override string TypeName => "double";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeDouble>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!TryGetValue(valueString, out double result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeDoubleArray : VariateTypeFloat , IVariateArray
    {
        public override string TypeName => "double[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeDoubleArray>(name, columnIndex);
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