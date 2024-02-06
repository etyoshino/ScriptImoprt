using System;
using System.Collections.Generic;
using System.IO;

namespace Engine.Excel
{
    public static class ExcelHelp
    {
        public static Object[] ToObjectArray<T>(this List<T> @this, Func<T, Object> func)
        {
            return ToArray(@this, func);
        }
        
        public static TValue[] ToArray<T, TValue>(this List<T> @this, Func<T, TValue> func)
        {
            TValue[] values = new TValue[@this.Count];
            for (int i = 0, iLength = @this.Count; i < iLength; i++)
            {
                values[i] = func(@this[i]);
            }
            return values;
        }
        
        public static void TryCreateDirectory(string strDirectoryPath)
        {
            if (!Directory.Exists(strDirectoryPath))
                Directory.CreateDirectory(strDirectoryPath);
        }

        public static void DeleteExcel(string assetPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(assetPath);
           
            
            // foreach (var configPair in configMgr.config.configs)
            // {
            //     if (configPair.configName == fileName)
            //     {
            //         string csfilePath = $"{ExcelCommonField.ConfigScriptsRootPath}CSV{fileName}Reader.cs";
            //         if (File.Exists(csfilePath))
            //         {
            //             File.Delete(csfilePath);
            //         }
            //
            //         var scvPath = configPair.excelPath.Replace("Excel", "CSV");
            //         scvPath = scvPath.Replace(ExcelCommonField.ExcelExtension, ".csv");
            //         if (File.Exists(scvPath))
            //         {
            //             File.Delete(scvPath);
            //         }
            //         break;
            //     }
            // }
            // configMgr.OnDeleteConfig(fileName);
        }
    }
}