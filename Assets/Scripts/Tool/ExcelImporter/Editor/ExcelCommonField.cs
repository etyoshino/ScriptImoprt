using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Engine.Excel
{
    class ExcelCommonField
    {
        public static readonly string ConfigCSVRootPath = "Assets/Config/CSV/";
        public static readonly string ConfigExcelRootPath = "Assets/Config/Excel/";
        public static readonly string ConfigScriptsRootPath = "Assets/Scripts/Generated/";
        public const string ExcelExtension = ".xlsx";
        public const string ConfigNameSpace = "Game.Config";
        
        public static readonly string ConfigSCTemplatePath = $"{GetDirectionPath()}/ConfigScriptTemplete.cs.txt";
        public static readonly Regex replaceReg = new (@"{#\w+#*}");
        
        public const char VariateTypeSplitChar = ';';
        
        public const int VariateNameRow = 0;
        public const int VariateTypeRow = 1;
        public const int VariateDescRow = 2; //配置描述
        public const int StartRowIndex = 3;
        
        public static Dictionary<string, VariateTypeBase> VariateDic;
        public static Dictionary<string, Type> EnumDic;

        static ExcelCommonField()
        {
            VariateDic = new();
            EnumDic = new();
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

            // RegisterVariateType(new VariateTypeVector2());
            // RegisterVariateType(new VariateTypeVector2Int());
            // RegisterVariateType(new VariateTypeVector2Array());
            // RegisterVariateType(new VariateTypeVector2IntArray());
            
            // RegisterVariateType(new VariateTypeVector3());
            // RegisterVariateType(new VariateTypeVector3Int());
            // RegisterVariateType(new VariateTypeVector3Array());
            // RegisterVariateType(new VariateTypeVector3IntArray());

            #endregion

            #region Enum

            UpdateEnumConfigDic();

            #endregion
        }
        
        /// <summary>
        /// 刷新 枚举类型字典
        /// </summary>
        public static void UpdateEnumConfigDic()
        {
            EnumDic.Clear();
            System.Reflection.Assembly assemblyCoreConfig = System.Reflection.Assembly.Load("Game");
            Type[] types = Array.FindAll(assemblyCoreConfig.GetTypes(), x => x.IsEnum && x.Namespace == ConfigNameSpace);
            for (var i = 0;  i < types.Length; i++)
            {
                EnumDic.Add(types[i].Name, types[i]);
            }
        }

        private static void RegisterVariateType(VariateTypeBase variateType)
        {
            if (variateType is null)
                return;

            VariateDic.Add(variateType.TypeName.ToUpper(), variateType);
            VariateDic.TryAdd(variateType.FullTypeName.ToUpper(), variateType);
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