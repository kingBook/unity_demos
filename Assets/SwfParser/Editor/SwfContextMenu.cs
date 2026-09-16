#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// .swf 文件环境菜单
/// </summary>
public class SwfContextMenu : Editor {

    /// <summary>
    /// .swf 文件扩展名
    /// </summary>
    private const string swfExtensionName = ".swf";

    /// <summary>
    /// 验证是 .swf 时，环境菜单才启用
    /// </summary>
    [MenuItem("Assets/Parse swf", true)]
    private static bool ValidateParseSwf() {
        if (EditorApplication.isPlaying) return false;

        for (int i = 0, len = Selection.assetGUIDs.Length; i < len; i++) {
            string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
            bool isSwf = path.EndsWith(swfExtensionName, true, null);
            if (isSwf) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  验证是 .swf 时，开始解析
    /// </summary>
    [MenuItem("Assets/Parse swf")]
    private static void ParseSwf() {
        if (EditorApplication.isPlaying) return;

        for (int i = 0, len = Selection.assetGUIDs.Length; i < len; i++) {
            string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
            bool isSwf = path.EndsWith(swfExtensionName, true, null);
            if (isSwf) {
                SwfProcessor.ParseSwf(path);
            }
        }
    }

}
#endif
