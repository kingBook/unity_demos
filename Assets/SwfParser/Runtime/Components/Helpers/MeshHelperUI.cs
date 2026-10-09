using UnityEngine;

namespace SwfParserRuntime {

    public class MeshHelperUI : MeshHelperBase {

        public MeshHelperUI(CanvasRenderer canvasRenderer) : base() {
            var mesh = new Mesh();
            mesh.SetVertices(new Vector3[]{
                new (0,0,0),
                new (100,0,0),
                new (100,100,0)
            });
            mesh.SetUVs(0, new Vector2[]{
                new (0,0),
                new (1,0),
                new (1,1)
            });
            mesh.SetTriangles(new int[] { 2, 1, 0 }, 0);
            mesh.SetNormals(new Vector3[]{
                Vector3.back,
                Vector3.back,
                Vector3.back
            });
            canvasRenderer.SetMesh(mesh);



            //Material whiteMat = new(Shader.Find("UI/Default"));
            Material redMat = new(Shader.Find("UI/Default"));
            redMat.SetColor("_Color", Color.red);
            canvasRenderer.SetMaterial(redMat, 0);
        }

    }
}