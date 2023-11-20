namespace Excel
{
    public class VariateTypeString : VariateTypeBase
    {
        public override string TypeName => "string";
        public override bool TryPrase(string valueString, int rowIdx, ref string logError)
        {
            return true;
        }
    }

    public class VariateTypeStringArray : VariateTypeString, IVariateArray
    {
        public override string TypeName => "string[]";

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