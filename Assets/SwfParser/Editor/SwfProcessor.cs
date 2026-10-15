#if UNITY_EDITOR
using SwfParserRuntime;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SwfParserEditor {

    public static class SwfProcessor {

        /// <summary>
        /// 解析 .swf
        /// </summary>
        /// <param name="swfPath"> "Assets/xx.swf" </param>
        public static void ParseSwf(string swfPath) {

            // 转为绝对路径, 如：E:/projects/unity_swfParse/Assets/xx.swf
            swfPath = FileUtil.GetLogicalPath(System.IO.Path.GetFullPath(swfPath));
            // 截取掉 /xx.swf 的文件夹路径，如：E:/projects/unity_swfParse/Assets
            string swfFolderPath = FileUtil.GetLogicalPath(System.IO.Path.GetDirectoryName(swfPath));

            // =========== 创建 Swf 类实例 ===================================
            Stopwatch sw = new();
            sw.Start();
            Swf swf = Swf.Create(swfPath);
            sw.Stop();
            Debug.Log("read swf passed time:" + sw.ElapsedMilliseconds);

            // =========== 导出 .xml ========================================
            if (SwfParseConfig.isExportXml) {
                SwfXmlExporter swfXmlExporter = new(swf);
                swfXmlExporter.Export(swfPath);
            }

            // =========== 导出位图 ==========================================
            AtlasData[] atlasDatas = null;
            switch (SwfParseConfig.exportImagesOption) {
                case ExportImagesOption.OneByOne:
                    SwfImagesExporter swfImagesExporter = new(swf);
                    atlasDatas = swfImagesExporter.Export(swfFolderPath);
                    break;
                case ExportImagesOption.Atlas:
                    SwfAtlasesExporter swfAtlasesExporter = new(swf);
                    atlasDatas = swfAtlasesExporter.Export(swfFolderPath);
                    break;
            }

            // =========== 导出运行时数据 (xx.swfData) ======================
            if (SwfParseConfig.isExportSwfData) {
                SwfAssetExporter swfAssetExporter = new(swf);
                swfAssetExporter.Export(swfPath, atlasDatas);
            }

            // ===============================================================
            AssetDatabase.Refresh();
            Debug.Log($"Parseing completed: {FileUtil.GetProjectRelativePath(swfPath)}");
            //EditorUtility.DisplayDialog("Complete", $"Import complete\n\n{FileUtil.GetProjectRelativePath(swfPath)}", "OK");
        }
    }
}
#endif