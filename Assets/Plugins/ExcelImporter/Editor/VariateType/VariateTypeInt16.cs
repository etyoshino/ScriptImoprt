namespace Excel
{
    public class VariateTypeInt16 : VariateTypeBase
    {
        public override string TypeName => "short";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt16>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out short result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeInt16Array : VariateTypeInt16 , IVariateArray
    {
        public override string TypeName => "short[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeInt16Array>(name, columnIndex);
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
    
    public class VariateTypeUInt16 : VariateTypeBase
    {
        public override string TypeName => "ushort";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt16>(name, columnIndex);
        }
        
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            if (!VariateHelp.TryGetValue(valueString, out short result))
            {
                logError += PraseLogError(rowIdx);
                return false;
            }

            return true;
        }
    }
    
    public class VariateTypeUInt16Array : VariateTypeUInt16 , IVariateArray
    {
        public override string TypeName => "ushort[]";
        
        public override VariateTypeBase CreateInstance(string name, int columnIndex)
        {
            return _CreateInstance<VariateTypeUInt16Array>(name, columnIndex);
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