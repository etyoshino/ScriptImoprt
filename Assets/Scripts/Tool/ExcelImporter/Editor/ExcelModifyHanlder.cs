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
                    ExcelHelp.DeleteExcel(oldPath);
                    AssetDatabase.Refresh();
                }
            }
            return AssetMoveResult.DidMove;
        }
    }
}

#endif