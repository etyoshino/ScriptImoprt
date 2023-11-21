namespace Excel
{
    public class VariateTypeInt64 : VariateTypeBase
    {
        public override string TypeName => "long";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt64>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out long result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeInt64Array : VariateTypeInt64 , IVariateArray
    {
        public override string TypeName => "long[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt64Array>(name, columnIndex);
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
    
    public class VariateTypeUInt64 : VariateTypeBase
    {
        public override string TypeName => "ulong";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt64>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out long result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeUInt64Array : VariateTypeUInt64 , IVariateArray
    {
        public override string TypeName => "ulong[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt64Array>(name, columnIndex);
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