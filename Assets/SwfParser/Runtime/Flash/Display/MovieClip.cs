using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SwfParserRuntime {

    public class MovieClip : Sprite {

        #region 非 Flash.Display.MovieClip 的属性
        [SerializeField, Tooltip("引用的 Swf")] private Swf m_swf;
        [SerializeField, Tooltip("链接类名")] private string m_symbolClassName;

        public Swf swf => m_swf;
        public string symbolClassName => m_symbolClassName;
        public DefineSpriteTag defineSpriteTag { get; private set; }
        public List<Tag>[] frameTags { get; private set; }
        #endregion

        public int currentFrame { get; set; }
        public string currentFrameLabel { get; set; }
        public string currentLabel { get; set; }
        public string[] currentLabels { get; set; }
        public bool isPlaying { get; set; }
        public int totalFrames { get; set; }

        public static MovieClip Create(Swf swf, string symbolClassName, DisplayObjectContainer parent, bool isInUI) {
            SymbolClassRecord symbol = System.Array.Find(swf.GetSymbols(), item => item.name == symbolClassName);
            GameObject gameObject = new(typeof(MovieClip).Name);
            gameObject.SetActive(false);
            MovieClip mc = gameObject.AddComponent<MovieClip>();
            mc.m_swf = swf;
            mc.m_symbolClassName = symbol.name;
            mc.defineSpriteTag = (DefineSpriteTag)swf.GetCharacterIdTag(symbol.tagId);
            mc.isInUI = isInUI;
            parent.AddChild(mc);
            gameObject.SetActive(true);
            return mc;
        }

        public static MovieClip Create(Swf swf, DefineSpriteTag defineSpriteTag, DisplayObjectContainer parent, bool isInUI) {
            GameObject gameObject = new(typeof(MovieClip).Name);
            gameObject.SetActive(false);
            MovieClip mc = gameObject.AddComponent<MovieClip>();
            mc.m_swf = swf;
            mc.m_symbolClassName = string.Empty;
            mc.defineSpriteTag = defineSpriteTag;
            mc.isInUI = isInUI;
            parent.AddChild(mc);
            gameObject.SetActive(true);
            return mc;
        }

        protected override void Awake() {
            // isInUI
            if(!isInUI && GetComponent<RectTransform>()!=null) {
                isInUI = true;
            }

            // 直接挂载时，defineSpriteTag 不会被赋值
            if (defineSpriteTag == null) {
                SymbolClassRecord symbol = System.Array.Find(swf.GetSymbols(), item => item.name == symbolClassName);
                defineSpriteTag = (DefineSpriteTag)swf.GetCharacterIdTag(symbol.tagId);
            }

            // 获取 defineSpriteTag 所有帧的 controlTags
            frameTags = GetFrameTags(defineSpriteTag);

            // 跳到指定帧
            GotoFrame(0);

            base.Awake();
        }

        /// <summary>
        /// 从指定帧开始播放。
        /// </summary>
        /// <param name="frame"></param>
        public void GotoAndPlay(int frame) {

        }

        /// <summary>
        /// 从指定帧标签开始播放。
        /// </summary>
        /// <param name="frameLabel"></param>
        public void GotoAndPlay(string frameLabel) {

        }

        /// <summary>
        /// 将播放头转到下一帧并停止。
        /// </summary>
        public void NextFrame() {

        }

        /// <summary>
        /// 在影片剪辑的时间轴中移动播放头。
        /// </summary>
        public void Play() {

        }

        /// <summary>
        /// 将播放头转到前一帧并停止。
        /// </summary>
        public void PrevFrame() {

        }

        /// <summary>
        /// 停止影片剪辑中的播放头。
        /// </summary>
        public void Stop() {

        }

        /// <summary>
        /// 获取一个 DefineSpriteTag 所有帧的 controlTags
        /// </summary>
        /// <param name="defineSpriteTag"></param>
        /// <returns> 返回一个二维列表，一维：帧索引；二维：当前帧的 controlTags </returns>
        private List<Tag>[] GetFrameTags(DefineSpriteTag defineSpriteTag) {
            int frameCount = defineSpriteTag.frameCount;
            var frameTags = new List<Tag>[frameCount];
            for (int i = 0; i < frameCount; i++) {
                frameTags[i] = new List<Tag>();
            }
            // 
            int frameIndex = 0;
            for (int i = 0, len = defineSpriteTag.controlTags.Length; i < len; i++) {
                var tag = defineSpriteTag.controlTags[i];
                var tagType = (TagType)tag.header.type;
                if (tagType == TagType.End) break;
                if (tagType == TagType.ShowFrame) {
                    frameIndex++;
                    continue; // 不添加 ShowFrame
                }
                frameTags[frameIndex].Add(tag);
            }
            return frameTags;
        }

        /// <summary>
        /// 跳至指定帧
        /// </summary>
        /// <param name="frameIndex"> 帧索引 </param>
        public void GotoFrame(int frameIndex) {
            frameIndex = Mathf.Clamp(frameIndex, 0, frameTags.Length - 1);
            List<Tag> tempFrameTags = frameTags[frameIndex];

            for (int i = 0, len = tempFrameTags.Count; i < len; i++) {
                var tag = tempFrameTags[i];
                tag.Load(swf, this, isInUI);
            }
        }

    }
}