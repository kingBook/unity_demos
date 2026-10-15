using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SwfParserRuntime {

    // [SerializeField] private Swf m_swf;
    // [SerializeField] private string m_symbolClassName;

    public class MovieClip : Sprite {

        public int currentFrame { get; set; }
        public string currentFrameLabel{ get; set; }
        public string currentLabel{ get; set; }
        public string[] currentLabels{ get; set; }
        public bool isPlaying{ get; set; }
        public int totalFrames{ get; set; }

        //
        private Swf m_swf;
        private List<Tag>[] m_frameTags;

        public MovieClip(Swf swf, string symbolClassName, bool isInUI) : base() {
            // 获取与类名匹配的 DefineSpriteTag
            DefineSpriteTag defineSpriteTag = swf.GetUsedDefineSpriteTag(symbolClassName);
            Init(swf, defineSpriteTag, isInUI);
        }

        public MovieClip(Swf swf, DefineSpriteTag defineSpriteTag, bool isInUI) : base() {
            Init(swf, defineSpriteTag, isInUI);
        }

        private void Init(Swf swf, DefineSpriteTag defineSpriteTag, bool isInUI) {
            m_swf = swf;
            base.isInUI = isInUI;

            // 获取 defineSpriteTag 所有帧的 controlTags
            m_frameTags = GetFrameTags(defineSpriteTag);

            // 跳到指定帧
            GotoFrame(0);
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
            frameIndex = Math.Clamp(frameIndex, 0, m_frameTags.Length - 1);
            List<Tag> frameTags = m_frameTags[frameIndex];

            for (int i = 0, len = frameTags.Count; i < len; i++) {
                var tag = frameTags[i];
                tag.Load(m_swf, this, isInUI);
            }
        }

    }
}