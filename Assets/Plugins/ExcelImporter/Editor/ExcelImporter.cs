using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Scriban;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Excel
{
    [ScriptedImporter(1, new[] { "xlsx" })]
    public class ExcelImporter : ScriptedImporter
    {
        public static readonly string ConfigCSVRootPath = "Assets/Config/CSV/";
        public static readonly string ConfigScriptsRootPath = "Assets/Scripts/Generate/";
        private static readonly string ConfigSCTemplatePath = $"{GetDirectionPath()}/ConfigScriptTemplete.cs.txt";

        internal const string CSNamespace = "CSVConfig";

        private const string ConfigLogWarring = "<===  Excel Warring  ===>\n";
        private const string ConfigLogError = "<===  Excel Error  ===>\n";
        private const string ConfigDescStr = "/// <summary>\n/// {0}\n/// </summary>";

        private const int VariateNameRow = 0;
        private const int VariateTypeRow = 1;
        private const int VariateDescRow = 2; //配置描述
        private const int StartRowIndex = 3;

        private static readonly Dictionary<string, VariateTypeBase> VariateDic;
        
        private static readonly HashSet<string> HashVariateNameCheck = new();

        private static readonly StringBuilder LogImportexcel = new StringBuilder(500);
        private static readonly StringBuilder stringBuilder = new StringBuilder(500);
        
        static ExcelImporter()
        {
            VariateDic = new();

            #region int

            RegisterVariateType(new VariateTypeInt8());
            RegisterVariateType(new VariateTypeUInt8());
            RegisterVariateType(new VariateTypeInt16());
            RegisterVariateType(new VariateTypeUInt16());
            RegisterVariateType(new VariateTypeInt());
            RegisterVariateType(new VariateTypeUInt());
            RegisterVariateType(new VariateTypeInt64());
            RegisterVariateType(new VariateTypeUInt64());


            RegisterVariateType(new VariateTypeInt8Array());
            RegisterVariateType(new VariateTypeUInt8Array());
            RegisterVariateType(new VariateTypeInt16Array());
            RegisterVariateType(new VariateTypeUInt16Array());
            RegisterVariateType(new VariateTypeIntArray());
            RegisterVariateType(new VariateTypeUIntArray());
            RegisterVariateType(new VariateTypeInt64Array());
            RegisterVariateType(new VariateTypeUInt64Array());

            #endregion

            #region customize

            RegisterVariateType(new VariateTypeString());
            RegisterVariateType(new VariateTypeBool());
            RegisterVariateType(new VariateTypeFloat());
            RegisterVariateType(new VariateTypeDouble());

            RegisterVariateType(new VariateTypeStringArray());
            RegisterVariateType(new VariateTypeBoolArray());
            RegisterVariateType(new VariateTypeFloatArray());
            RegisterVariateType(new VariateTypeDoubleArray());

            #endregion

            #region vector

            RegisterVariateType(new VariateTypeVector2());
            RegisterVariateType(new VariateTypeVector2Int());
            RegisterVariateType(new VariateTypeVector2Array());
            RegisterVariateType(new VariateTypeVector2IntArray());


            RegisterVariateType(new VariateTypeVector3());
            RegisterVariateType(new VariateTypeVector3Int());
            RegisterVariateType(new VariateTypeVector3Array());
            RegisterVariateType(new VariateTypeVector3IntArray());

            #endregion
        }

        private static void RegisterVariateType(VariateTypeBase variateType)
        {
            if (variateType is null)
                return;

            VariateDic.Add(variateType.TypeName.ToUpper(), variateType);
            VariateDic.TryAdd(variateType.FullTypeName.ToUpper(), variateType);
        }
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (Path.GetFileName(ctx.assetPath).StartsWith("~$"))
            {
                return;
            }
            
            ImportExcel(ctx.assetPath);
            ctx.LogImportError(LogImportexcel.ToString());
        }
        
        bool ImportExcel(string path)
        {
            LogImportexcel.Clear();
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

            try
            {
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                var dataSet = excelDataReader.AsDataSet();

                int sheetCount = dataSet.Tables.Count;
                // string excelName = Path.GetFileNameWithoutExtension(path);
                string configName;
                // for (int i = 0; i < sheetCount; i++)
                {
                    DataTable dataTable = dataSet.Tables[0];
                    // configName = sheetCount > 1 ? excelName + dataTable.TableName : excelName;
                    configName = Path.GetFileNameWithoutExtension(path);
                    int rowCount = dataTable.Rows.Count;
                    int columnCount = dataTable.Columns.Count;
                    DataRowCollection dataRowCollection = dataTable.Rows;

                    bool isSuccess = ConfigCheck(rowCount, columnCount, dataRowCollection);
                    if (isSuccess)
                    {
                        isSuccess = GenerateConfigCS(configName, rowCount, columnCount, dataRowCollection);
                    }

                    if (isSuccess)
                    {
                        GenerateCSV(configName, rowCount, columnCount, dataRowCollection);
                    }
                    
                    stream.Close();
                    File.Delete(tempDataPath);
                }
                
                return true;
            }
            catch (Exception e)
            {
                //TODO
                stream.Close();
                File.Delete(tempDataPath);
                throw;
            }
            
        }
        
        private bool GenerateConfigCS(string configName, int rowCount, int columnCount, DataRowCollection dataRowCollection)
        {
            HashVariateNameCheck.Clear();

            List<VariateTypeBase> variateTypeList = new();
            VariateTypeBase tmpConfigVariate;
            string configVariateDesc;
            for (int columnIdx = 0; columnIdx < columnCount; columnIdx++)
            {
                string configVariateName = dataRowCollection[VariateNameRow][columnIdx].ToString().Trim();
                if (string.IsNullOrEmpty(configVariateName))
                {
                    LogImportexcel.Append($"{ConfigLogWarring}Config:{configName} Column:{columnIdx} Variate Empty\n");
                    break;
                }

                if (HashVariateNameCheck.Contains(configVariateName))
                {
                    LogImportexcel.Append($"{ConfigLogError}Config:{configName} Column:{columnIdx} Repeat VariateName\n");
                    break;
                }

                HashVariateNameCheck.Add(configVariateName);

                string configVariateType = dataRowCollection[VariateTypeRow][columnIdx].ToString().ToUpper();
                if (VariateDic.TryGetValue(configVariateType, out tmpConfigVariate))
                {
                    configVariateDesc = dataRowCollection[VariateDescRow][columnIdx].ToString().Trim();

                    tmpConfigVariate = tmpConfigVariate.CreateInstance(configVariateName, columnIdx);
                    dataRowCollection[VariateTypeRow][columnIdx] = tmpConfigVariate.FullTypeName;
                    variateTypeList.Add(tmpConfigVariate);
                    tmpConfigVariate.VariateDesc = string.IsNullOrEmpty(configVariateDesc)
                        ? configVariateDesc
                        : string.Format(ConfigDescStr, configVariateDesc);

                    string tmpErrorStr;
                    for (int rowIdx = StartRowIndex; rowIdx < rowCount; rowIdx++)
                    {
                        tmpErrorStr = "";
                        var configVaule = dataRowCollection[rowIdx][columnIdx].ToString().Trim();

                        if (!tmpConfigVariate.TryPrase(configVaule, rowIdx, ref tmpErrorStr))
                        {
                            LogImportexcel.Append(tmpErrorStr);
                            return false;
                        }
                    }
                }
                else
                {
                    LogImportexcel.Append($"{ConfigLogError}Config:{configName} Column:{columnIdx} Error VariateType:{configVariateType}\n");
                }
            }

            Template csTemplate = Template.Parse(File.ReadAllText(ConfigSCTemplatePath));
            string csText = csTemplate.Render(new
            {
                cs_namespace = CSNamespace,
                config_name = configName,
                
                fields = variateTypeList.ToObjectArray(variateType => new
                {
                    index = variateType.ColumnIndex,
                    desc = variateType.VariateDesc,
                    variate_type = variateType.FullTypeName,
                    variate_name = variateType.Name,
                }),
            });
            
            TryCreateDirectory(ConfigScriptsRootPath);
            
            string csFilePath = $"{ConfigScriptsRootPath}{configName}.cs";
            File.WriteAllText(csFilePath, csText);
            return true;
        }

        private void TryCreateDirectory(string strDirectoryPath)
        {
            if (!Directory.Exists(strDirectoryPath))
                Directory.CreateDirectory(strDirectoryPath);
        }
        
        private bool ConfigCheck(int rowCount, int columnCount, DataRowCollection dataRowCollection)
        {
            if (columnCount < 2 || rowCount <= StartRowIndex)
            {
                return false;
            }
            // 默认第一行为key
            if (dataRowCollection[VariateTypeRow][0].ToString().Contains("[]"))
            {
                LogImportexcel.Append($"{ConfigLogError} Array cannot be used as key");
                return false;
            }
            return true;
        }
        
        private void GenerateCSV(string conifgName, int rowCount, int columnCount, DataRowCollection dataRowCollection)
        {
            TryCreateDirectory(ConfigCSVRootPath);
            string csvPath = $"{ConfigCSVRootPath}{conifgName}.csv";

            stringBuilder.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    string content = dataRowCollection[i][j].ToString();
                    if (content.Contains(','))
                    {
                        stringBuilder.Append('"');
                        stringBuilder.Append(content);
                        stringBuilder.Append('"');
                    }
                    else
                        stringBuilder.Append(content);
                    stringBuilder.Append(',');
                }
                stringBuilder.Append('\n');
            }
            
            if (File.Exists(csvPath))
            {
                File.SetAttributes(csvPath, FileAttributes.Normal);
            }

            try
            {
                File.WriteAllText(csvPath, stringBuilder.ToString(), Encoding.UTF8);
                File.SetAttributes(csvPath,FileAttributes.ReadOnly);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ConfigLogError} generate CSV:{csvPath} Failed/{e}");
                throw;
            }
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