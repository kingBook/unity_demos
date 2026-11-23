
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


            var (scaleX, scaleY, skewX, skewY, tx, ty, rotation) = DecomposeFlashMatrix(matrix);
            Debug.Log($"scaleX:{scaleX}, scaleY:{scaleY}, skewX:{skewX}, skewY:{skewY}, tx:{tx}, ty:{ty}, rotation:{rotation}");

        }

        public static void RectTransformApplyMatrix(UnityEngine.RectTransform rectTransform, SwfParserRuntime.Matrix matrix) {

        }

        public static (float scaleX, float scaleY, float skewX, float skewY, float tx, float ty, float rotation) DecomposeFlashMatrix(SwfParserRuntime.Matrix m) {
            float sx = Mathf.Sqrt(m.a * m.a + m.b * m.b);
            float ky = Mathf.Atan2(m.b, m.a);
            if (sx != 0 && Mathf.Abs(ky) > Mathf.PI / 2) {
                sx = -sx;
                ky += ky > 0 ? -Mathf.PI : Mathf.PI;
            }

            float sy = Mathf.Sqrt(m.c * m.c + m.d * m.d);
            float kx = Mathf.Atan2(-m.c, m.d);
            float det = m.a * m.d - m.b * m.c;
            float cosDiff = Mathf.Cos(ky - kx);
            if (sx != 0 && cosDiff != 0) {
                sy = det / (sx * cosDiff);
            }

            return (
                scaleX: sx,
                scaleY: sy,
                skewX: kx * 180 / Mathf.PI,
                skewY: ky * 180 / Mathf.PI,
                tx: m.tx,
                ty: m.ty,
                rotation: Mathf.Atan2(m.b, m.a) * 180 / Mathf.PI
            );
        }
    }

}