using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;

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
        
        public static bool ConfigRowEmpty(DataRow row, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                var str = row[i].ToString();
                if (!string.IsNullOrEmpty(str))
                    return false;
            }

            return true;
        }
        
        public static void MoveExcel(string fileName, string newPath)
        {
            // TODO
        }

        public static void DeleteExcel(string assetPath, bool isReName = false)
        {
            // TODO
        }
    }
}