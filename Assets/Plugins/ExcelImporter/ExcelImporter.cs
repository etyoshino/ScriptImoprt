using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;

namespace Excel
{
    [ScriptedImporter(1, new[] { "xlsx" })]
    public class ExcelImporter : ScriptedImporter
    {
        private static readonly string ConfigScriptsRootPath = "";
        private static readonly string ConfigRootPath = "";

        private const int VariateNameRow = 0;
        private const int VariateTypeRow = 1;
        private const int VariateDescRow = 2;   //配置描述
        private const int StartRowIndex = 3;
        
        private static readonly StringBuilder _stringBuilder = new StringBuilder(500);

        static ExcelImporter()
        {
            
        }
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (Path.GetFileName(ctx.assetPath).StartsWith("~$"))
            {
                return;
            }

            GenerateConfigReader(ctx.assetPath);
        }

        private bool GenerateConfigReader(string path)
        {
            FileStream stream = null;
            const string tempDataPath = "Temp/TempConfigData";
            try
            {
                File.Copy(path, tempDataPath, true);
                stream = File.Open(tempDataPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch
            {
                return false;
            }

            using (var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var dataSet = excelDataReader.AsDataSet();

                DataTable dataTable = dataSet.Tables[0];
                int rowCount = dataTable.Rows.Count;
                int columnCount = dataTable.Columns.Count;
                DataRowCollection dataRowCollection = dataTable.Rows;

                bool isSuccess = ConfigCheck(rowCount, columnCount, dataRowCollection);
                if (isSuccess)
                {
                    isSuccess = GenerateConfigCS(path,columnCount,dataRowCollection);
                }

                return isSuccess;
            }
            return true;
        }

        private bool GenerateConfigCS(string path, int columnCount, DataRowCollection dataRowCollection)
        {
            string directionPath = Path.GetDirectoryName(path);
            var configName = $"{Path.GetFileNameWithoutExtension(path)}Reader";
            
            
            return true;
        }

        private bool ConfigCheck(int rowCount, int columnCount, DataRowCollection dataRowCollection)
        {
            if (columnCount < 2 || rowCount <= StartRowIndex)
            {
                return false;
            }
            
            
            return true;
        }
        
    }

}