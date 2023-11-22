namespace Excel
{
    public class VariateTypeBool : VariateTypeBase
    {
        public override string TypeName => "bool";

        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeBool>(name, columnIndex);
        }

        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!TryGetValue(valueString, out bool result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }

    public class VariateTypeBoolArray : VariateTypeBool
    {
        public override string TypeName => "bool[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeBoolArray>(name, columnIndex);
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