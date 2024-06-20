using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Game.Config;
using Excel;
using Scriban;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Engine.Excel
{
    [ScriptedImporter(1, new[] { "xlsx" })]
    class ExcelImporter : ScriptedImporter
    {
        internal const string CSNamespace = "Core.Config";

        private const string ConfigLogWarring = "===  Excel Warring  ===\n";
        private const string ConfigLogError = "===  Excel Error  ===\n";
        private const string ScriptNameFormat = "CT{0}";
        private const string ScriptFileNameFormat = "CT{0}Reader";
        
        private static readonly HashSet<string> HashVariateNameCheck = new();
        
        private static readonly StringBuilder LogImportexcel = new StringBuilder(500);
        private static readonly StringBuilder stringBuilder = new StringBuilder(1000);
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (Path.GetFileName(ctx.assetPath).StartsWith("~$"))
            {
                return;
            }
            
            if (ImportExcel(ctx.assetPath))
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
            else
            {
                LogImportexcel.Append(ctx.assetPath+"\n");
                throw new Exception(LogImportexcel.ToString());
            }
        }
        
        public bool ImportExcel(string path)
        {
            LogImportexcel.Clear();
            var configName = Path.GetFileNameWithoutExtension(path);
            
            FileStream stream = null;
            const string tempDataPath = "Temp/TempConfigData";
            try
            {
                if (File.Exists(tempDataPath))  //标记为临时文件
                {
                    File.SetAttributes(tempDataPath,FileAttributes.Temporary);
                }
                File.Copy(path, tempDataPath, true);
                stream = File.Open(tempDataPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                File.SetAttributes(tempDataPath,FileAttributes.Temporary);      //设置为临时文件
            }
            catch(Exception e)
            {
                LogImportexcel.Append(e);
                return false;
            }

            var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dataSet = excelDataReader.AsDataSet();
            var directory = Path.GetDirectoryName(path).Replace('\\','/') + '/';
            var pathFlag = directory.Contains(ExcelCommonField.ConfigExcelRootPath);
            var csvPath = pathFlag
                ? directory.Replace(ExcelCommonField.ConfigExcelRootPath, ExcelCommonField.ConfigCSVRootPath)
                : ExcelCommonField.ConfigCSVRootPath;
            if (!pathFlag)
            {
                Debug.LogError($"Please Save Excel:{ path } in the Paht:{ ExcelCommonField.ConfigCSVRootPath }");
            }
            
            // int sheetCount = dataSet.Tables.Count;
            // string excelName = Path.GetFileNameWithoutExtension(path);
            // string configName;
            // for (int i = 0; i < sheetCount; i++)
            {
                // configName = Path.GetFileNameWithoutExtension(path);
                DataTable dataTable = dataSet.Tables[0];
                // configName = sheetCount > 1 ? excelName + dataTable.TableName : excelName;
                int rowCount = dataTable.Rows.Count;
                int columnCount = dataTable.Columns.Count;
                DataRowCollection dataRowCollection = dataTable.Rows;

                bool isSuccess = ConfigCheck(rowCount, columnCount, dataRowCollection);
                if (isSuccess)
                {
                    isSuccess = GenerateConfigCS(configName, rowCount, columnCount, dataRowCollection);
                    if (isSuccess)
                    {
                        GenerateCSV(configName, path ,csvPath , rowCount, columnCount, dataRowCollection);
                    }
                }
                stream.Close();
                File.Delete(tempDataPath);
                
                return isSuccess;
            }
        }
        
        
        private static bool GenerateConfigCS(string configName, int rowCount, int columnCount, DataRowCollection dataRowCollection)
        {
            HashVariateNameCheck.Clear();
            
            List<VariateTypeBase> variateTypeList = new();
            Dictionary<string, HashSet<string>> variateEnumList = new();
            VariateTypeBase tmpConfigVariate;
            string configVariateDesc;
            // 检查配置表数据格式是否正确
            for (int columnIdx = 0; columnIdx < columnCount; columnIdx++)
            {
                string configVariateName = dataRowCollection[ExcelCommonField.VariateNameRow][columnIdx].ToString().Trim();
                if (string.IsNullOrEmpty(configVariateName))
                {
                    LogImportexcel.Append($"{ConfigLogWarring}Config:<{configName}> Column:<{columnIdx + 1}> Variate Empty\n");
                    break;
                }

                if (HashVariateNameCheck.Contains(configVariateName))
                {
                    LogImportexcel.Append($"{ConfigLogError}Config:<{configName}> Column:<{columnIdx + 1}> Repeat VariateName\n");
                    break;
                }
                HashVariateNameCheck.Add(configVariateName);

                var originTypeName = dataRowCollection[ExcelCommonField.VariateTypeRow][columnIdx].ToString();
                var variateTypes = originTypeName.Split(ExcelCommonField.VariateTypeSplitChar);
                string configVariateType = variateTypes[0];
                var enumCheck = VariateEnumBase.EnumCheck(configVariateType, configVariateName, columnIdx,
                    out tmpConfigVariate);
                configVariateType = configVariateType.ToUpper();

                do
                {
                    if (enumCheck){}
                    else if (ExcelCommonField.VariateDic.TryGetValue(configVariateType, out tmpConfigVariate))
                    {
                        tmpConfigVariate = tmpConfigVariate.CreateInstance(configVariateName, columnIdx);
                    }
                    else
                    {
                        LogImportexcel.Append($"{ConfigLogError}Config:<{configName}> Column:<{columnIdx + 1}> Error VariateType:{configVariateType}\n");
                        break;
                    }
                    
                    configVariateDesc = dataRowCollection[ExcelCommonField.VariateDescRow][columnIdx].ToString().Trim();
                    SetConfigVariateAttribute(variateTypes, tmpConfigVariate);

                    tmpConfigVariate.SourceTypeName = originTypeName;
                    tmpConfigVariate.VariateDesc = configVariateDesc;
                    variateTypeList.Add(tmpConfigVariate);

                    if (!TryParseVariateData(tmpConfigVariate, columnIdx, rowCount, dataRowCollection))
                        return false;
                    
                    if (enumCheck)
                    {
                        var enumVariate = tmpConfigVariate as VariateEnumBase;
                        if (enumVariate.CreateType)
                        {
                            if (!variateEnumList.ContainsKey(enumVariate.EnumName))
                            {
                                variateEnumList[enumVariate.EnumName] = new HashSet<string>();
                            }
                            variateEnumList[enumVariate.EnumName].AddRange(enumVariate.EnumKeys);
                        }
                    }
                }while(false);
                
            }
            
            // Build CS Render
            Template csTemplate = Template.Parse(File.ReadAllText(ExcelCommonField.ConfigSCTemplatePath));
            string csText = csTemplate.Render(new
            {
                cs_namespace = CSNamespace,
                class_name = string.Format(ScriptNameFormat, configName),
                start_row = ExcelCommonField.StartRowIndex,
                config_manager = $"CSV{configName}Manager",
                baseclass_name = nameof(CTItemBase),
                csv_managerbase = nameof(CTManagerBase),

                fields = variateTypeList.ToObjectArray(variateType => new
                {
                    variate_name = variateType.Name,
                    variate_type = variateType.FullTypeName,
                    base_type = variateType.BaseTypeName,
                    variate_split_char = variateType.ArrarySplitChar,
                    source_fulltype = variateType.SourceTypeName,

                    variate_name_upper = variateType.Name,
                    variate_cstype = variateType.CSTypeName,
                        
                    create_type = variateType.IsUnmanagedType,
                    index = variateType.ColumnIndex,
                    desc = variateType.VariateDesc,
                    key = variateType.VariateAttribute.HasFlag(EVariateAttribute.Key),
                    union = variateType.VariateAttribute.HasFlag(EVariateAttribute.Union),
                    ignore = variateType.VariateAttribute.HasFlag(EVariateAttribute.Ignore),
                }),
                enums = variateEnumList.Select(variateType => new
                {
                    name = variateType.Key,
                    values = variateType.Value.ToArray(),
                }).ToArray()
            });
            
            // 创建脚本文件
            ExcelHelp.TryCreateDirectory(ExcelCommonField.ConfigScriptsRootPath);
            string csFilePath = $"{ExcelCommonField.ConfigScriptsRootPath}{string.Format(ScriptFileNameFormat, configName)}.cs";
            if (File.Exists(csFilePath))
            {
                File.WriteAllText(csFilePath, csText);
            }
            else
            {
                File.WriteAllText(csFilePath, csText);
            }
            
            // 更新枚举库
            if (variateEnumList.Count() > 0)
            {
                ExcelCommonField.UpdateEnumConfigDic();
            }
            return true;
        }
        
        /// <summary>
        /// 解析配置表数据 解析失败 终止导表
        /// </summary>
        /// <returns></returns>
        private static bool TryParseVariateData(VariateTypeBase tmpConfigVariate, int columnIdx, int rowCount,
            DataRowCollection dataRowCollection)
        {
            StringBuilder tmpErrorStr = new();
            for (int rowIdx = ExcelCommonField.StartRowIndex; rowIdx < rowCount; rowIdx++)
            {
                tmpErrorStr.Clear();
                var configVaule = dataRowCollection[rowIdx][columnIdx].ToString().Trim();
                if (!tmpConfigVariate.TryParse(configVaule, rowIdx, ref tmpErrorStr))
                {
                    LogImportexcel.Append(tmpErrorStr);
                    return false;
                }
                dataRowCollection[rowIdx][columnIdx] = configVaule;
            }
            
            return true;
        }

        /// <summary>
        /// 设置配置参数 特性
        /// </summary>
        private static void SetConfigVariateAttribute(string[] variateTypes, VariateTypeBase tmpConfigVariate)
        {
            for (int i = 1; i < variateTypes.Length; i++)
            {
                tmpConfigVariate.UpdateVariateAttribute(variateTypes[i]);
            }
        }
        
        private static bool ConfigCheck(int rowCount, int columnCount, DataRowCollection dataRowCollection)
        {
            //允许空表的创建
            if (columnCount < 2 || rowCount <= ExcelCommonField.VariateDescRow)
            {
                LogImportexcel.Append($"{ConfigLogError} Config Fromat Error");
                return false;
            }
            //默认第一行为key
            if (dataRowCollection[ExcelCommonField.VariateTypeRow][0].ToString().Contains("[]"))
            {
                LogImportexcel.Append($"{ConfigLogError} Array cannot be used as key");
                return false;
            }
            //不用检查2、3行 类型不存在 会生成空cs文件不影响运行
            return true;
        }
        
        private static void GenerateCSV(string configName, string excelPath, string csvDirectory, int rowCount,
            int columnCount, DataRowCollection dataRowCollection)
        {
            ExcelHelp.TryCreateDirectory(csvDirectory);
            string csvPath = $"{csvDirectory}{configName}.csv";
            
            stringBuilder.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                // 过滤掉空行和不需要的行数据
                if (i > ExcelCommonField.StartRowIndex && ExcelHelp.ConfigRowEmpty(dataRowCollection[i], columnCount))
                {
                    continue;
                }
                
                for (int j = 0; j < columnCount; j++)
                {
                    // 把文本中的换行符替换为换行
                    string content = dataRowCollection[i][j]
                        .ToString()
                        .Replace("\n", "\\n");
                    
                    if (content.Contains('\"'))
                    {
                        content = content.Replace("\"","\"\"");
                        stringBuilder.Append('"');
                        stringBuilder.Append(content);
                        stringBuilder.Append('"');
                    }
                    else if (content.Contains(','))
                    {
                        stringBuilder.Append('"');
                        stringBuilder.Append(content);
                        stringBuilder.Append('"');
                    }
                    else
                        stringBuilder.Append(content);
                    
                    if(j < columnCount - 1)
                        stringBuilder.Append(',');
                }
                stringBuilder.Append('\r');
                stringBuilder.Append('\n');
            }
            
            if (File.Exists(csvPath))
            {
                File.WriteAllText(csvPath, stringBuilder.ToString(), Encoding.UTF8);
            }
            else
            {
                File.WriteAllText(csvPath, stringBuilder.ToString(), Encoding.UTF8);
                AssetDatabase.ImportAsset(csvPath);
            }
        }
    }
}