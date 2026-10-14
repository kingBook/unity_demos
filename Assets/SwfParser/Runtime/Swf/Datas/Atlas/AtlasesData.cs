using UnityEngine;

namespace SwfParserRuntime {

    [System.Serializable]
    public class AtlasesData {

        public Texture2D[] atlasAssets;
        public string[] atlasPaths;
        public RectInfoArray[] rectInfo2Ds;

        public AtlasData GetAtlasData(ushort characterID) {
            Texture2D atlas = null;
            SubSpriteInfo? rectInfo = null;
            string atlasPath = null;

            for (int i = 0, len = rectInfo2Ds.Length; i < len; i++) {
                var rectInfos = rectInfo2Ds[i].rectInfos;
                bool isFound = false;
                for (int j = 0, c = rectInfos.Length; j < c; j++) {
                    var retInfo = rectInfos[j];
                    if (characterID == retInfo.characterID) {
                        isFound = true;
                        atlas = atlasAssets[i];
                        atlasPath = atlasPaths[i];
                        rectInfo = retInfo;
                        break;
                    }
                }
                if (isFound) break;
            }
            return new AtlasData() {
                sourceTex = atlas,
                path = atlasPath,
                //subSpriteInfo = rectInfo
            };
        }


    }
}