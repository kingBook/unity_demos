using SwfParserRuntime;
using UnityEngine;

public class TestApplyMatrix : MonoBehaviour {

    public Transform iconTransform;

    private void Awake() {
        // (a=-0.3824005126953125, b=-0.28887939453125, c=-0.81707763671875, d=0.5936431884765625, tx=150.5, ty=89.75)
        var m = new Matrix() { a = -0.3824005126953125f, b = -0.28887939453125f, c = -0.81707763671875f, d = 0.5936431884765625f, tx = 150.5f, ty = 89.75f };

        MatrixUtil.TransformApplyMatrix(iconTransform, m);
    }
}
