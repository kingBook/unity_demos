using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SwfParserRuntime {

    public class Shape : DisplayObject {

        //public Graphics graphics;

        // public Mesh mesh { get; protected set; }
        // public Material material { get; protected set; }

        public Swf swf { get; private set; }
        public DefineShapeTag defineShapeTag { get; private set; }

        public static Shape Create(Swf swf, DefineShapeTag defineShapeTag, DisplayObjectContainer parent, bool isInUI) {
            GameObject gameObj = new(typeof(Shape).Name);
            gameObj.SetActive(false);

            Shape shape = gameObj.AddComponent<Shape>();
            shape.swf = swf;
            shape.defineShapeTag = defineShapeTag;
            shape.isInUI = isInUI;
            parent.AddChild(shape);

            gameObj.SetActive(true);
            
            return shape;
        }

        // public Shape(SubSpriteInfo subSpriteInfo, Matrix matrix) : base() {
        // Texture2D atlas = atlasData.sourceTex;
        // string atlasPath = atlasData.path;
        // SubSpriteInfo rectInfo = atlasData.subSpriteInfo.Value;

        // mesh = new Mesh();

        // // 顶点 -------------------------------------
        // float w = rectInfo.rect.width * atlas.width;
        // float h = rectInfo.rect.height * atlas.height;
        // Vector3[] vertices = new Vector3[4] {
        //     matrix.TransformPoint(new(0, 0)),
        //     matrix.TransformPoint(new(0, h)),
        //     matrix.TransformPoint(new(w, h)),
        //     matrix.TransformPoint(new(w, 0))
        // };
        // mesh.SetVertices(vertices);

        // // UV -------------------------------------------
        // Vector2[] uvs = new Vector2[4] {
        //     new(rectInfo.rect.x, rectInfo.rect.y),
        //     new(rectInfo.rect.x, rectInfo.rect.y + rectInfo.rect.height),
        //     new(rectInfo.rect.x + rectInfo.rect.width, rectInfo.rect.y + rectInfo.rect.height),
        //     new(rectInfo.rect.x + rectInfo.rect.width, rectInfo.rect.y)
        // };
        // mesh.SetUVs(0, uvs);

        // // 法线 ------------------------------------------
        // Vector3[] normals = new Vector3[4] {
        //     Vector3.back,
        //     Vector3.back,
        //     Vector3.back,
        //     Vector3.back
        // };
        // mesh.SetNormals(normals);

        // // 三角形 ----------------------------------------
        // int[] triangles = new int[3 * 2] {
        //     0,1,3,
        //     1,2,3Packages/com.unity.render-pipelines.universal/Shaders/2D/Sprite-Unlit-Default.shader
        // };
        // mesh.SetTriangles(triangles, 0);

        // // 材质 ------------------------------------------
        // material = new(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default")) {
        //     mainTexture = atlas
        // };
        // }

        protected override void Awake() {
            // bitmapId
            FillStyleRecord[] fillStyles = defineShapeTag.shapes.fillStyles.fillStyles;
            if (fillStyles.Length >= 2) {
                FillStyleRecord fillStyle = fillStyles[1];
                byte type = fillStyle.fillStyleType;
                if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
                    SubSpriteInfo subSpriteInfo = swf.FindSubSpriteInfoInAtlasDatas(fillStyle.bitmapId);
                    // Shape shape = new(subSpriteInfo, fillStyle.bitmapMatrix.ToMatrix());
                    Debug.Log($"bitmapMatrix {fillStyle.bitmapMatrix.ToMatrix()}");
                    if (isInUI) {
                        Image image = gameObject.AddComponent<Image>();
                        image.sprite = subSpriteInfo.sprite;
                        image.rectTransform.sizeDelta = subSpriteInfo.rect.size;
                    } else {
                        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                        spriteRenderer.sprite = subSpriteInfo.sprite;
                    }
                }
            }
            base.Awake();
        }

    }
}