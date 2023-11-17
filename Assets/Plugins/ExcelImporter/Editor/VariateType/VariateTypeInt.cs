namespace Excel
{
    public class VariateTypeInt8 : VariateTypeBase
    {
        public override string TypeName => "sbyte";
    }
    
    public class VariateTypeInt16 : VariateTypeBase
    {
        public override string TypeName => "short";
    }
    
    public class VariateTypeInt : VariateTypeBase
    {
        public override string TypeName => "int";
    }
    
    public class VariateTypeInt64 : VariateTypeBase
    {
        public override string TypeName => "long";
    }
}