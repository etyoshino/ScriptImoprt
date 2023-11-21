namespace Excel
{
    public class VariateTypeInt8 : VariateTypeBase
    {
        public override string TypeName => "sbyte";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt8>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out sbyte result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeInt8Array : VariateTypeInt8 , IVariateArray
    {
        public override string TypeName => "sbyte[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt8Array>(name, columnIndex);
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
    
    public class VariateTypeUInt8 : VariateTypeBase
    {
        public override string TypeName => "byte";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt8>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out sbyte result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeUInt8Array : VariateTypeUInt8 , IVariateArray
    {
        public override string TypeName => "byte[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt8Array>(name, columnIndex);
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