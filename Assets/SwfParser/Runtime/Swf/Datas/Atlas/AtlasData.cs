using UnityEngine;

namespace SwfParserRuntime {

    /// <summary> 单张图集数据 </summary>
    [System.Serializable]
    public class AtlasData {

        /// <summary> 图集路径 </summary>
        public string path;

        /// <summary> 图集源纹理 </summary>
        public Texture2D sourceTex;

        /// <summary> 图集中的子 Sprite 信息 </summary>
        public SubSpriteInfo[] subSpriteInfos;

    }
}