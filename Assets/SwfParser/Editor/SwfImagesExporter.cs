using SwfParserRuntime;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
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
            // 获取所有位图数据
            Stopwatch sw = new();
            sw.Start();
            ImageData[] imageDatas = m_swf.GetImageDatas(true);
            sw.Stop();
            Debug.Log("GetImageDatas passed time:" + sw.ElapsedMilliseconds);

            // 创建图集
            sw.Restart();
            CreateAtlases(imageDatas, out Texture2D[] atlases, out SubSpriteInfo[] subSpriteInfos);
            WriteImageDatasToDisk(imageDatas, swfFolderPath, out string[] atlasPaths);
            ImportAtlasAssets(atlasPaths, ref subSpriteInfos, out Texture2D[] atlasAssets);

            // * 到此处 imageDatas， atlases, atlasPaths, subSpriteInfos, atlasAssets 的长度应相等的

            // 图集数据
            int len = atlasPaths.Length;
            AtlasData[] atlasDatas = new AtlasData[len];
            for (int i = 0; i < len; i++) {
                atlasDatas[i] = new() {
                    path = atlasPaths[i],
                    sourceTex = atlasAssets[i],
                    subSpriteInfos = new SubSpriteInfo[] { subSpriteInfos[i] }
                };
            }
            sw.Stop();
            Debug.Log("Save image datas passed time:" + sw.ElapsedMilliseconds);

            return atlasDatas;
        }

        private void ImportAtlasAssets(string[] atlasPaths, ref SubSpriteInfo[] subSpriteInfos, out Texture2D[] atlasAssets) {
            // Sprite 数据工厂
            SpriteDataProviderFactories factory = new();
            factory.Init();

            int len = atlasPaths.Length;
            atlasAssets = new Texture2D[len];
            for (int i = 0; i < len; i++) {
                string path = atlasPaths[i];
                SubSpriteInfo info = subSpriteInfos[i];

                // 导入图集
                AssetDatabase.ImportAsset(path);

                // 获取图集
                Texture2D atlasAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                // int atlasW = atlasAsset.width, atlasH = atlasAsset.height;

                // 导入器
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritePixelsPerUnit = 100;
                importer.filterMode = FilterMode.Bilinear;
                importer.wrapMode = TextureWrapMode.Clamp;

                // 设置 SpriteRect
                ISpriteEditorDataProvider dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
                dataProvider.InitSpriteEditorDataProvider();
                SpriteRect[] spriteRects = new SpriteRect[] {
                    new () {
                        name = $"{info.characterID}",
                        rect = info.rect,
                        pivot = new Vector2(0.5f, 0.5f)
                    }
                };
                dataProvider.SetSpriteRects(spriteRects);
                dataProvider.Apply();

                // 保存并重新导入
                importer.SaveAndReimport();

                // 完善 SubSpriteInfo
                Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                info.sprite = subAssets[0] as UnityEngine.Sprite; // 只一个子 Sprite，直接取[0]
                subSpriteInfos[i] = info; // 更新

                // 导入后的图集纹理路径
                atlasAssets[i] = atlasAsset;
            }
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
                    rect = new Rect(0, 0, texture2D.width, texture2D.height), // 非单位化
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
        /// <param name="atlasPaths"> 输出, 保存图集后的路径  </param>
        private void WriteImageDatasToDisk(ImageData[] imageDatas, string swfFolderPath, out string[] atlasPaths) {
            int len = imageDatas.Length;
            atlasPaths = new string[len];
            for (int i = 0; i < len; i++) {
                ImageData imageData = imageDatas[i];
                if (imageData.bytes == null || imageData.bytes.Length <= 0) {
                    throw new System.Exception("Image data is empty or invalid for characterID " + imageData.characterID + ".");
                }
                string extensionName = imageData.type == ImageType.Png ? ".png" : ".jpg";
                string path = $"{swfFolderPath}/{imageData.characterID}{extensionName}";
                FileStream fs = new(path, FileMode.Create);
                fs.Write(imageData.bytes, 0, imageData.bytes.Length);
                fs.Close();
                atlasPaths[i] = FileUtil.GetProjectRelativePath(path);
            }
        }

    }
}