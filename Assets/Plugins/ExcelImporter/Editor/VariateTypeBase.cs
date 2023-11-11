namespace Excel
{
    abstract class VariateTypeBase
    {
        public string variateName
        {
            get;
            private set;
        }
        
        public string variateType
        {
            get;
            private set;
        }

        public int columnCount;

        public char splitChar;

        public virtual void Init(string name,string type,char splitchar = ';')
        {
            
        }
    }
}