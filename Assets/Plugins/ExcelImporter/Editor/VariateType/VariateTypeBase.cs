namespace Excel
{
    public abstract class VariateTypeBase
    {
        public abstract string TypeName { get; }
        
        public string Name;
        
        public int ColumnIndex;

        public char SplitChar;
        
        public char ArrarySplitChar;
        
        // public abstract VariateTypeBase CreateInstance(string name, int columnIndex, char splitChar = ';');
        public static T CreateInstance<T>(string name, int columnIndex, char splitChar = ';')
            where T : VariateTypeBase, new()
        {
            T result = new T();
            result.InitData(name, columnIndex, splitChar);
            return result;
        }
        
        public void InitData(string name,int columnIndex, char splitChar = ';')
        {
            Name = name;
            ColumnIndex = columnIndex;
            SplitChar = splitChar;
        }
    }
}