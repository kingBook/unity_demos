using UnityEngine;

namespace SwfParserEditor {

    public enum ExportImagesOption {
        /// <summary> 单张 </summary>
        OneByOne,
        /// <summary> 图集 </summary>
        Atlas
    }

    public static class SwfParseConfig {

        /// <summary> 是否导出 .swfData </summary>
        public static bool isExportSwfData = true;

        /// <summary> 是否导出 .xml </summary>
        public static bool isExportXml = false;

        /// <summary> 导出图片选项 </summary>
        public static ExportImagesOption exportImagesOption = ExportImagesOption.Atlas;

        /// <summary> 当导出为图集时，允许的纹理大小上限 </summary>
        public static int exportMaxAtlasSize = 2048;

        /// <summary> 当导出为图集时，图块间距 </summary>
        public static int exportAtlaspadding = 4;

        // /// <summary> 默认材质 </summary>
        // public static Material defaultMaterial = new (Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));

    }
}