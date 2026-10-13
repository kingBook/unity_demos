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
        /// </summary>
        /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
        /// <returns></returns>
        public AtlasesData Export(string swfFolderPath) {
            // 获取所有图像数据
            Stopwatch sw = new();
            sw.Start();
            ImageData[] imageDatas = m_swf.GetImageDatas(true);
            sw.Stop();
            Debug2.Log("GetImageDatas passed time:" + sw.ElapsedMilliseconds);

            // 创建图集和数据
            sw.Restart();
            CreateAtlases(imageDatas, SwfParseConfig.exportMaxAtlasSize, SwfParseConfig.exportAtlaspadding, out Texture2D[] atlases, out RectInfoArray[] rectInfo2Ds);
            var atlasPaths = WriteTexture2DsToDisk(atlases, swfFolderPath); // 已打包的所有图集保存到硬盘
            var atlasAssets = ImportAtlasAssets(atlasPaths, rectInfo2Ds); // 将已保存到硬盘的图集导入到编辑器，并返回导入后的图集纹理路径
            sw.Stop();
            Debug2.Log("Save swfSpriteAtlas passed time:" + sw.ElapsedMilliseconds);

            return new AtlasesData {
                atlasPaths = atlasPaths,
                rectInfo2Ds = rectInfo2Ds,
                atlasAssets = atlasAssets
            };
        }

        /// <summary>
        /// 将图集导入到编辑器
        /// </summary>
        /// <param name="atlasPaths"> 图集路径数组 </param>
        /// <param name="rectInfo2Ds"> 打包后各个图集中所有图块的矩形框信息（二维数组） </param>
        /// <returns> 导入后的图集纹理路径 </returns>
        private Texture2D[] ImportAtlasAssets(string[] atlasPaths, RectInfoArray[] rectInfo2Ds) {
            // Sprite 数据工厂
            var factory = new SpriteDataProviderFactories();

            int len = atlasPaths.Length;
            var atlasAssets = new Texture2D[len];
            for (int i = 0; i < len; i++) {
                var path = atlasPaths[i];

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
                factory.Init();
                ISpriteEditorDataProvider dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
                dataProvider.InitSpriteEditorDataProvider();
                SpriteRect[] rects = rectInfo2Ds[i].rectInfos.Select(rectInfo => {
                    return new SpriteRect() {
                        name = $"{rectInfo.characterID}",
                        // 由于 rectInfo.rect 中的数值是已单位化的，所以需乘以图集的宽/高
                        rect = new Rect(rectInfo.rect.x * atlasW, rectInfo.rect.y * atlasH, rectInfo.rect.width * atlasW, rectInfo.rect.height * atlasH),
                        pivot = new Vector2(0.5f, 0.5f)
                    };
                }).ToArray();
                dataProvider.SetSpriteRects(rects);
                dataProvider.Apply();

                // 保存并重新导入
                importer.SaveAndReimport();

                // 导入后的图集纹理路径
                atlasAssets[i] = atlasAsset;
            }
            return atlasAssets;
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
        /// <param name="atlases"> 输出 -> 打包后的图集数组 </param>
        /// <param name="rectInfo2Ds"> 输出 -> 打包后各个图集中所有图块的矩形框信息（二维数组） </param>
        private void CreateAtlases(ImageData[] imgDatas, int maxAtlasSize, int padding, out Texture2D[] atlases, out RectInfoArray[] rectInfo2Ds) {
            // 图片数据数组 -> Texture2D数组
            Texture2D[] texture2Ds = imgDatas.Select(imgData => {
                var texture2D = new Texture2D(4, 4);
                texture2D.LoadImage(imgData.bytes);
                return texture2D;
            }).ToArray();

            // 图集配置列表（计算出需要打包多少张图集， 一个图集配置对应一张图集）
            List<AtlasConfig> atlasConfigs = Texture2DsToAtlasConfigs(texture2Ds, maxAtlasSize, padding);

            // 创建图集和数据
            int len = atlasConfigs.Count;
            atlases = new Texture2D[len];
            rectInfo2Ds = new RectInfoArray[len];
            for (int i = 0; i < len; i++) {
                var cfg = atlasConfigs[i];

                // 创建大图集纹理
                var texture2DAtlas = new Texture2D(cfg.size, cfg.size);

                // 将一张图集需打包的纹理拷贝到 subTexture2Ds
                var subTexture2Ds = new Texture2D[cfg.endIndex - cfg.startIndex + 1];
                System.Array.Copy(texture2Ds, cfg.startIndex, subTexture2Ds, 0, subTexture2Ds.Length);

                // subTexture2Ds 打包到图集
                var rects = texture2DAtlas.PackTextures(subTexture2Ds, cfg.padding);

                // 当前图集中所有图块的矩形框信息数组
                var rectInfos = new RectInfoArray(cfg.endIndex - cfg.startIndex + 1);
                for (int j = cfg.startIndex; j <= cfg.endIndex; j++) {
                    var rectInfo = new RectInfo {
                        characterID = imgDatas[j].characterID,
                        rect = rects[j - cfg.startIndex]
                    };
                    rectInfos[j - cfg.startIndex] = rectInfo;
                }

                // 输出
                atlases[i] = texture2DAtlas; // 图集
                rectInfo2Ds[i] = rectInfos; // 矩形框信息
            }
        }

        /// <summary>
        /// 将纹理保存到硬盘
        /// </suddmmary>
        /// <param name="texture2Ds"> 将要保存到硬盘的纹理 </param>
        /// <param name="swfFolderPath">  文件夹路径（绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets） </param>
        /// <returns> 返回保存后的路径数组 </returns>
        private string[] WriteTexture2DsToDisk(Texture2D[] texture2Ds, string swfFolderPath) {
            var len = texture2Ds.Length;
            var paths = new string[len];
            for (int i = 0; i < len; i++) {
                var bytes = texture2Ds[i].EncodeToPNG();
                string path = $"{swfFolderPath}/{i}.png";
                FileStream fs = new(path, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                paths[i] = FileUtil.GetProjectRelativePath(path);
            }
            return paths;
        }

    }
}