
using UnityEngine;

namespace SwfParserRuntime {

    public static class MatrixUtil {

        public static void TransformApplyMatrix(UnityEngine.Transform transform, SwfParserRuntime.Matrix matrix) {
            // 应用矩阵：先缩放 → 再倾斜 → 再旋转 → 最后平移
            // 缩放
            //if (hasScale) {
            transform.localScale = new Vector3(matrix.a / 20f, matrix.d / 20f, 1.0f);
            //}

            // 旋转
            //if (hasRotate) {
            // spriteRenderer.transform.localRotation
            //}
            // 平移(注意: ty 取反方向)
            transform.Translate(new Vector3(matrix.tx, -matrix.ty, 0.0f), Space.Self);


            var (scaleX, scaleY, rotation, skewX, skewY) = Decompose(matrix);
            Debug.Log($"scaleX:{scaleX}, scaleY:{scaleY}, rotation:{rotation}, skewX:{skewX}, skewY:{skewY}");

        }

        public static void RectTransformApplyMatrix(UnityEngine.RectTransform rectTransform, SwfParserRuntime.Matrix matrix) {

        }

        public static (float scaleX, float scaleY, float rotation, float skewX, float skewY) Decompose(SwfParserRuntime.Matrix m) {
            float sx = Mathf.Sqrt(m.a * m.a + m.c * m.c);
            float sy = Mathf.Sqrt(m.b * m.b + m.d * m.d);
            float rot = Mathf.Atan2(m.c, m.a);
            float skewX = Mathf.Atan2(m.d, m.b) - rot;
            float skewY = Mathf.Atan2(m.c, m.a) - rot;
            return (
                scaleX: sx, scaleY: sy,
                rotation: rot * 180 / Mathf.PI,
                skewX: skewX * 180 / Mathf.PI,
                skewY: skewY * 180 / Mathf.PI
            );
        }
    }

}