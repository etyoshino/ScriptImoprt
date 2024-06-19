#if UNITY_EDITOR

using System.IO;
using UnityEditor;

namespace Engine.Excel
{
    class ExcelModifyHanlder : AssetModificationProcessor
    {
        static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (Path.GetExtension(assetPath).Equals(ExcelCommonField.ExcelExtension))
            {
                ExcelHelp.DeleteExcel(assetPath);
            }
        
            return AssetDeleteResult.DidNotDelete;
        }

        public static AssetMoveResult OnWillMoveAsset(string oldPath,string newPath)
        {
            if (Path.GetExtension(newPath).Equals(ExcelCommonField.ExcelExtension))
            {
                var oldFileName = Path.GetFileNameWithoutExtension(oldPath);
                var newFileName = Path.GetFileNameWithoutExtension(newPath);
                if (!string.Equals(oldFileName, newFileName))
                {
                    // 重命名会触发 ScriptedImporter 直接删除旧文件
                    ExcelHelp.DeleteExcel(oldPath, true);
                }
                else
                {
                    // 改文件名不会触发 ScriptedImporter 直接修改CSV文件路径
                    ExcelHelp.MoveExcel(newFileName, newPath);
                }
            }
            return AssetMoveResult.DidMove;
        }
    }
}

#endif