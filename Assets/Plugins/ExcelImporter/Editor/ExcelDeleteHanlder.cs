#if UNITY_EDITOR

using System.IO;
using UnityEditor;

namespace Excel
{
    
    public class ExcelDeleteHanlder : AssetModificationProcessor
    {
        private const string excelExtension = ".xlsx";
        static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (Path.GetExtension(assetPath).Equals(excelExtension))
            {
                var fileName = Path.GetFileNameWithoutExtension(assetPath);
                string csfilePath = $"{ExcelImporter.ConfigScriptsRootPath}{fileName}.cs";
                string csvfilePath = $"{ExcelImporter.ConfigCSVRootPath}{fileName}.csv";
                File.Delete(csfilePath);
                File.Delete(csvfilePath);
            }
        
            return AssetDeleteResult.DidNotDelete;
        }
    }
}

#endif