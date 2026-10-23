using System.Linq;
using UnityEngine;

namespace SwfParserRuntime {

    public class Shape : DisplayObject {

        //public Graphics graphics;

        public Mesh mesh { get; protected set; }
        public Material material { get; protected set; }

        public static Shape Create(DisplayObjectContainer parent, bool isInUI) {
            GameObject gameObj = new(typeof(Shape).Name);
            Shape shape = gameObj.AddComponent<Shape>();
            shape.isInUI = isInUI;
            parent.AddChild(shape);
            return shape;
        }

        public Shape(SubSpriteInfo subSpriteInfo, Matrix matrix) : base() {


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


        }

    }
}