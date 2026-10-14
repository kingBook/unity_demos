namespace SwfParserRuntime {

    [System.Serializable]
    public class RectInfoArray {

        public SubSpriteInfo[] rectInfos;

        public RectInfoArray(int length) {
            rectInfos = new SubSpriteInfo[length];
        }

        public SubSpriteInfo this[int index] {
            get { return rectInfos[index]; }
            set { rectInfos[index] = value; }
        }
    }
}