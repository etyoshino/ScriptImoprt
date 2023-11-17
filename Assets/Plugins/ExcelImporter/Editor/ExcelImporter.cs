using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Scriban;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Excel
{
    [ScriptedImporter(1, new[] { "xlsx" })]
    public class ExcelImporter : ScriptedImporter
    {
        private static readonly string ConfigScriptsRootPath = "Assets/Scripts/Generate/";
        private static readonly string ConfigSCTemplatePath = $"{GetDirectionPath()}/ConfigScriptTemplete.cs.txt";

        private const int VariateNameRow = 0;
        private const int VariateTypeRow = 1;
        private const int VariateDescRow = 2;   //配置描述
        private const int StartRowIndex = 3;

        private static readonly HashSet<string> HashVariateNameCheck; 
        
        internal const string CSNamespace = "CSVConfig";
        
        private static readonly string ConfigDescStr = " /// <summary>\n/// {1}/// </summary>";
        
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
            var configName = Path.GetFileNameWithoutExtension(path);

            List<VariateTypeBase> variateTypeList = new();
            for (int columnIdx = 0; columnIdx < columnCount; columnIdx++)
            {
                string variateName = dataRowCollection[VariateNameRow][columnIdx].ToString().Trim();
                if (string.IsNullOrEmpty(variateName))
                {
                    Debug.LogWarning($"=== Excel {configName} Column:{columnIdx} Variate Empty");
                    break;
                }
                
                string variateType = dataRowCollection[VariateTypeRow][columnIdx].ToString().Trim();
                // VariateTypeBase variateConfig = new VariateTypeBase();
                // variateConfig.SetData(variateName, columnIdx);
                //
                // variateTypeList.Add(variateConfig);
            }
            
            Template csTemplate = Template.Parse(File.ReadAllText(ConfigSCTemplatePath));
            string csText = csTemplate.Render(new
            {
                cs_namespace = CSNamespace,
                config_name = configName,
                //templ
                baseclass_name = "CSVReaderBase",
                fields = variateTypeList.ToObjectArray(variateType => new
                {
                    index = variateType.ColumnIndex,
                    desc = "",
                    variate_type = variateType.TypeName,
                    variate_name = variateType.Name,
                    
                }),
            });
            
            string csFilePath = $"{ConfigScriptsRootPath}{configName}.cs";
            
            File.WriteAllText(csFilePath, csText);
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
        
        private static string GetFilePath([CallerFilePath] string path = "")
        {
            return path;
        }
        
        private static string GetDirectionPath()
        {
            return Path.GetDirectoryName(GetFilePath());
        }
    }

}