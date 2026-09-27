using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CustomUVGraphic : MaskableGraphic {

    [SerializeField] private UnityEngine.Sprite sprite;

    // 指定渲染贴图，UGUI会自动合批
    public override Texture mainTexture {
        get {
            if (sprite != null)
                return sprite.texture;
            return s_WhiteTexture;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear(); // 清空原有网格，必须写

        Rect rect = rectTransform.rect;
        float w = rect.width;
        float h = rect.height;

        // 4个顶点位置（UI坐标，原点在Rect中心）
        Vector2 pos0 = new Vector2(-w / 2, -h / 2);
        Vector2 pos1 = new Vector2(w / 2, -h / 2);
        Vector2 pos2 = new Vector2(w / 2, h / 2);
        Vector2 pos3 = new Vector2(-w / 2, h / 2);

        // ========== 自定义UV区域 ==========
        // 如果使用Sprite，要用DataUtility.GetOuterUV获取sprite真实uv，不要直接0~1
        Vector4 spriteUV = sprite ? DataUtility.GetOuterUV(sprite) : new Vector4(0, 0, 1, 1);
        float uMin = spriteUV.x;
        float vMin = spriteUV.y;
        float uMax = spriteUV.z;
        float vMax = spriteUV.w;

        // 四个顶点UV
        Vector2 uv0 = new Vector2(uMin, vMin);
        Vector2 uv1 = new Vector2(uMax, vMin);
        Vector2 uv2 = new Vector2(uMax, vMax);
        Vector2 uv3 = new Vector2(uMin, vMax);

        Color vertColor = color;

        // 添加顶点
        vh.AddVert(pos0, vertColor, uv0);
        vh.AddVert(pos1, vertColor, uv1);
        vh.AddVert(pos2, vertColor, uv2);
        vh.AddVert(pos3, vertColor, uv3);

        // 构造2个三角形，组成矩形
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
    }

    // 当属性变化时，强制重建网格
    protected override void OnRectTransformDimensionsChange() {
        base.OnRectTransformDimensionsChange();
        SetVerticesDirty();
    }

    protected override void OnValidate() {
        base.OnValidate();
        SetVerticesDirty();
    }
}
