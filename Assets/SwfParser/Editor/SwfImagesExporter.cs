using SwfParserRuntime;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SwfParserEditor {

    public class SwfImagesExporter {

        private readonly Swf m_swf;

        public SwfImagesExporter(Swf swf) {
            m_swf = swf;
        }

        /// <summary>
        /// 导出位图数据
        /// </summary>
        /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
        public AtlasData[] Export(string swfFolderPath) {
            Stopwatch sw = new();
            sw.Start();
            ImageData[] imageDatas = m_swf.GetImageDatas(true);
            sw.Stop();
            Debug.Log("GetImageDatas passed time:" + sw.ElapsedMilliseconds);

            sw.Restart();
            CreateAtlases(imageDatas, out _, out SubSpriteInfo[] subSpriteInfos);
            var atlasPaths = WriteImageDatasToDisk(imageDatas, swfFolderPath);
            var atlasAssets = ImportAtlasAssets(atlasPaths);
            // var atlasesData = new AtlasesData {
            //     atlasAssets = atlasAssets,
            //     atlasPaths = atlasPaths,
            //     rectInfo2Ds = rectInfo2Ds
            // };
            sw.Stop();
            Debug.Log("Save image datas passed time:" + sw.ElapsedMilliseconds);

            return null;
        }

        private Texture2D[] ImportAtlasAssets(string[] atlasPaths) {
            int len = atlasPaths.Length;
            var atlasAssets = new Texture2D[len];
            for (int i = 0; i < len; i++) {
                var path = atlasPaths[i];
                AssetDatabase.ImportAsset(path);
                atlasAssets[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
            return atlasAssets;
        }

        private void CreateAtlases(ImageData[] imgDatas, out Texture2D[] atlases, out SubSpriteInfo[] subSpriteInfos) {
            // 图片数据数组 -> Texture2D数组
            atlases = imgDatas.Select(imgData => {
                Texture2D texture2D = new(4, 4);
                texture2D.LoadImage(imgData.bytes);
                return texture2D;
            }).ToArray();

            // 子 Sprite 信息
            int len = imgDatas.Length;
            subSpriteInfos = new SubSpriteInfo[len];
            for (int i = 0; i < len; i++) {
                ushort characterID = imgDatas[i].characterID;
                Texture2D texture2D = atlases[i];

                SubSpriteInfo subSpriteInfo = new() {
                    characterID = characterID,
                    rect = new Rect(0, 0, texture2D.width, texture2D.height),
                    sprite = null // 后面再赋值
                };
                subSpriteInfos[i] = subSpriteInfo;
            }
        }

        /// <summary>
        /// 以png或jpg，保存位图数据到本地（如果 bytes==null 或 bytes.length<=0 将取消）
        /// </summary>
        /// <param name="imageDatas"> 位图数据 </param>
        /// <param name="swfFolderPath"> 绝对路径，如：E:/projects/unity_swfParse/Assets/test_export </param>
        private string[] WriteImageDatasToDisk(ImageData[] imageDatas, string swfFolderPath) {
            var len = imageDatas.Length;
            var paths = new string[len];
            for (int i = 0; i < len; i++) {
                ImageData imageData = imageDatas[i];
                if (imageData.bytes == null || imageData.bytes.Length <= 0) continue;
                string extensionName = imageData.type == ImageType.Png ? ".png" : ".jpg";
                string path = $"{swfFolderPath}/{imageData.characterID}{extensionName}";
                FileStream fs = new(path, FileMode.Create);
                fs.Write(imageData.bytes, 0, imageData.bytes.Length);
                fs.Close();
                paths[i] = FileUtil.GetProjectRelativePath(path);
            }
            return paths;
        }

    }
}