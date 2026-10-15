using SwfParserRuntime;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace SwfParserEditor {

    /// <summary>
    /// Swf 图集导出器
    /// </summary>
    public class SwfAtlasesExporter {

        /// <summary> 图集配置 </summary>
        private struct AtlasConfig {

            /// <summary> 开始索引 </summary>
            public int startIndex;

            /// <summary> 结束索引 </summary>
            public int endIndex;

            /// <summary> 大小 </summary>
            public int size;

            /// <summary> 间距 </summary>
            public int padding;
        }

        private readonly Swf m_swf;

        public SwfAtlasesExporter(Swf swf) {
            m_swf = swf;
        }


        /// <summary>
        /// 导出位图数据
        /// * 注：整个 Swf 导出到的一个图集数据数组
        /// </summary>
        /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
        /// <returns></returns>
        public AtlasData[] Export(string swfFolderPath) {
            // 获取所有位图数据
            Stopwatch sw = new();
            sw.Start();
            ImageData[] imageDatas = m_swf.GetImageDatas(true);
            sw.Stop();
            Debug2.Log("GetImageDatas passed time:" + sw.ElapsedMilliseconds);

            // 创建图集
            sw.Restart();
            CreateAtlases(imageDatas, SwfParseConfig.exportMaxAtlasSize, SwfParseConfig.exportAtlaspadding, out Texture2D[] atlases, out SubSpriteInfo[][] subSpriteInfo2Ds);
            WriteTexture2DsToDisk(atlases, swfFolderPath, out string[] atlasPaths); // 已打包的所有图集保存到硬盘
            ImportAtlasAssets(atlasPaths, ref subSpriteInfo2Ds, out Texture2D[] atlasAssets); // 将已保存到硬盘的图集导入到编辑器，并返回导入后的图集纹理路径

            // * 到此处 atlases, atlasPaths, subSpriteInfo2Ds, atlasAssets 的长度应相等的，但与 imageDatas 的长度不一定相等

            // 图集数据
            int len = atlasPaths.Length;
            AtlasData[] atlasDatas = new AtlasData[len];
            for (int i = 0; i < len; i++) {
                atlasDatas[i] = new() {
                    path = atlasPaths[i],
                    sourceTex = atlasAssets[i],
                    subSpriteInfos = subSpriteInfo2Ds[i]
                };
            }
            sw.Stop();
            Debug2.Log("Save swfSpriteAtlas passed time:" + sw.ElapsedMilliseconds);
            return atlasDatas;
        }

        /// <summary>
        /// 将图集导入到编辑器
        /// </summary>
        /// <param name="atlasPaths"> 图集路径数组 </param>
        /// <param name="subSpriteInfo2Ds"> 打包后各个图集中的子 Sprite 信息（二维数组） </param>
        /// <param name="atlasAssets"> 导入后的图集纹理路径 </param>
        private void ImportAtlasAssets(string[] atlasPaths, ref SubSpriteInfo[][] subSpriteInfo2Ds, out Texture2D[] atlasAssets) {
            // Sprite 数据工厂
            SpriteDataProviderFactories factory = new();
            factory.Init();

            int len = atlasPaths.Length;
            atlasAssets = new Texture2D[len];
            for (int i = 0; i < len; i++) {
                string path = atlasPaths[i];
                SubSpriteInfo[] subSpriteInfos = subSpriteInfo2Ds[i];

                // 导入图集
                AssetDatabase.ImportAsset(path);

                // 获取图集
                Texture2D atlasAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                int atlasW = atlasAsset.width, atlasH = atlasAsset.height;

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
                SpriteRect[] spriteRects = subSpriteInfos.Select(info => {
                    return new SpriteRect() {
                        name = $"{info.characterID}",
                        // 由于 info.rect 中的数值是已单位化的，所以需乘以图集的宽/高
                        rect = new Rect(info.rect.x * atlasW, info.rect.y * atlasH, info.rect.width * atlasW, info.rect.height * atlasH),
                        pivot = new Vector2(0.5f, 0.5f)
                    };
                }).ToArray();
                dataProvider.SetSpriteRects(spriteRects);
                dataProvider.Apply();

                // 保存并重新导入
                importer.SaveAndReimport();

                // 完善 SubSpriteInfo
                Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                for (int j = 0, c = subSpriteInfos.Length; j < c; j++) {
                    SubSpriteInfo info = subSpriteInfos[j];

                    // 由于 info.rect 中的数值是已单位化的，所以需乘以图集的宽/高
                    info.rect.x *= atlasW;
                    info.rect.y *= atlasH;
                    info.rect.width *= atlasW;
                    info.rect.height *= atlasH;

                    info.sprite = subAssets[j] as UnityEngine.Sprite;

                    subSpriteInfos[j] = info; // 更新
                }

                // 导入后的图集纹理路径
                atlasAssets[i] = atlasAsset;
            }
        }

        /// <summary>
        /// 将要被打包纹理数组转换为图集配置
        /// * 注：当一张图集无法容纳所有纹理时，会分成多张，所以最后返回的是一个图集配置数组
        /// </summary>
        /// <param name="texture2Ds"> 将要被打包纹理 </param>
        /// <param name="maxAtlasSize"> 图集大小上限 </param>
        /// <param name="padding"> 图块间距 </param>
        /// <returns></returns>
        private List<AtlasConfig> Texture2DsToAtlasConfigs(Texture2D[] texture2Ds, int maxAtlasSize, int padding) {
            var atlasConfigs = new List<AtlasConfig>();

            int areaTotal = 0, startIndex = 0;
            for (int i = 0, len = texture2Ds.Length; i < len; i++) {
                var texture2D = texture2Ds[i];

                int area = texture2D.width * texture2D.height;
                areaTotal += area;
                int sqrt = Mathf.CeilToInt(Mathf.Sqrt(areaTotal));
                int nextPowerOfTwo = Mathf.NextPowerOfTwo(sqrt);
                nextPowerOfTwo = Mathf.Min(nextPowerOfTwo, maxAtlasSize);

                if (nextPowerOfTwo >= maxAtlasSize) {
                    var config = new AtlasConfig {
                        startIndex = startIndex,
                        endIndex = i,
                        size = nextPowerOfTwo,
                        padding = padding
                    };
                    atlasConfigs.Add(config);
                    startIndex = i + 1;
                    areaTotal = 0;
                } else if (i >= len - 1) { // 最后一个
                    if (startIndex <= i) {
                        var config = new AtlasConfig {
                            startIndex = startIndex,
                            endIndex = i,
                            size = nextPowerOfTwo,
                            padding = padding
                        };
                        atlasConfigs.Add(config);
                    }
                }
            }
            return atlasConfigs;
        }

        /// <summary>
        /// 创建图集
        /// </summary>
        /// <param name="imgDatas"> 图片数据数组（characterID、 Jpg/Png、 字节数组） </param>
        /// <param name="maxAtlasSize"> 图集大小上限 </param>
        /// <param name="padding"> 图块间距 </param>
        /// <param name="atlases"> 输出，打包后的图集数组 </param>
        /// <param name="subSpriteInfo2Ds"> 输出，打包后各个图集中的子 Sprite 信息 </param>
        private void CreateAtlases(ImageData[] imgDatas, int maxAtlasSize, int padding, out Texture2D[] atlases, out SubSpriteInfo[][] subSpriteInfo2Ds) {
            // 图片数据数组 -> Texture2D数组
            Texture2D[] texture2Ds = imgDatas.Select(imgData => {
                Texture2D texture2D = new(4, 4);
                texture2D.LoadImage(imgData.bytes);
                return texture2D;
            }).ToArray();

            // 图集配置列表（计算出需要打包多少张图集， 一个图集配置对应一张图集）
            List<AtlasConfig> atlasConfigs = Texture2DsToAtlasConfigs(texture2Ds, maxAtlasSize, padding);

            // 创建图集和数据
            int len = atlasConfigs.Count;
            atlases = new Texture2D[len];
            subSpriteInfo2Ds = new SubSpriteInfo[len][];
            for (int i = 0; i < len; i++) {
                AtlasConfig cfg = atlasConfigs[i];

                // 创建大图集纹理
                Texture2D texture2DAtlas = new(cfg.size, cfg.size);

                // 将一张图集需打包的纹理拷贝到 subTexture2Ds
                Texture2D[] subTexture2Ds = new Texture2D[cfg.endIndex - cfg.startIndex + 1];
                System.Array.Copy(texture2Ds, cfg.startIndex, subTexture2Ds, 0, subTexture2Ds.Length);

                // subTexture2Ds 打包到图集
                Rect[] rects = texture2DAtlas.PackTextures(subTexture2Ds, cfg.padding);

                // 当前图集中所有的子 Sprite 信息
                SubSpriteInfo[] subSpriteInfos = new SubSpriteInfo[cfg.endIndex - cfg.startIndex + 1];
                for (int j = cfg.startIndex; j <= cfg.endIndex; j++) {
                    SubSpriteInfo subSpriteInfo = new() {
                        characterID = imgDatas[j].characterID,
                        rect = rects[j - cfg.startIndex], // 此处的rect，是单位化的
                        sprite = null // 后面再赋值
                    };
                    subSpriteInfos[j - cfg.startIndex] = subSpriteInfo;
                }

                // 输出
                atlases[i] = texture2DAtlas; // 图集
                subSpriteInfo2Ds[i] = subSpriteInfos; // 子 Sprite 信息
            }
        }

        /// <summary>
        /// 将纹理保存到硬盘
        /// </suddmmary>
        /// <param name="texture2Ds"> 将要保存到硬盘的纹理 </param>
        /// <param name="swfFolderPath">  文件夹路径（绝对路径，如：E:/projects/unity_swfParse/Assets/test_export）</param>
        /// <param name="atlasPaths"> 输出, 保存图集后的路径  </param>
        private void WriteTexture2DsToDisk(Texture2D[] texture2Ds, string swfFolderPath, out string[] atlasPaths) {
            int len = texture2Ds.Length;
            atlasPaths = new string[len];
            for (int i = 0; i < len; i++) {
                byte[] bytes = texture2Ds[i].EncodeToPNG();
                string path = $"{swfFolderPath}/{i}.png";
                FileStream fs = new(path, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                atlasPaths[i] = FileUtil.GetProjectRelativePath(path);
            }
        }

    }
}