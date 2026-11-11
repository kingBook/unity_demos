
using UnityEngine;

namespace SwfParserRuntime {

    public static class MatrixUtil {

        public static void TransformApplyMatrix(UnityEngine.Transform transform, SwfParserRuntime.Matrix matrix) {
            // 应用矩阵：先缩放 → 再倾斜 → 再旋转 → 最后平移
            // 缩放
            //if (hasScale) {
                transform.localScale = new Vector3(matrix.a, matrix.d, 1.0f);
            //}

            // 旋转
            //if (hasRotate) {
                // spriteRenderer.transform.localRotation
            //}
            // 平移(注意: ty 取反方向)
            transform.Translate(new Vector3(matrix.tx, -matrix.ty, 0.0f), Space.Self);

        }

        public static void RectTransformApplyMatrix(UnityEngine.RectTransform rectTransform, SwfParserRuntime.Matrix matrix) {

        }
    }

}