using UnityEngine;

namespace SwfParserRuntime {

    /// <summary>
    /// 图集中的子 Sprite 信息
    /// </summary>
    [System.Serializable]
    public struct SubSpriteInfo {

        /// <summary> 编号 </summary>
        public ushort characterID;

        /// <summary> 像素矩形 </summary>
        public Rect rect;

        /// <summary> 子 Spirte </summary>
        public UnityEngine.Sprite sprite;

    }
}