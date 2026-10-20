using UnityEditor;
using UnityEngine;
using System.Linq;

namespace SwfParserRuntime {

    [CustomEditor(typeof(MovieClip))]
    public class MovieClipEditor : Editor {

        /// <summary> 序列化属性 m_swf, 与 <see cref="MovieClip"/> 对应 </summary>
        private SerializedProperty m_swf;

        /// <summary> 序列化属性 m_spriteId, 与 <see cref="MovieClip"/> 对应 </summary>
        private SerializedProperty m_spriteId;

        private void OnEnable() {
            m_swf = serializedObject.FindProperty("m_swf");
            m_spriteId = serializedObject.FindProperty("m_spriteId");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update(); // 更新序列化对象

            // m_swf 属性
            EditorGUILayout.PropertyField(m_swf);


            // m_spriteId 属性
            MovieClip mc = (MovieClip)target;
            SymbolClassRecord[] symbols = mc.swf.GetSymbols();

            if (symbols == null || symbols.Length == 0) {
                EditorGUILayout.HelpBox("没有可用的 Symbol", MessageType.Info);
                serializedObject.ApplyModifiedProperties();  // 应用修改
                return;
            }

            int currentIndex = System.Array.FindIndex(symbols, item => item.tagId == m_spriteId.uintValue);
            if (currentIndex < 0) currentIndex = 0;

            string[] names = symbols.Select(item => $"{item.tagId} ({(!string.IsNullOrEmpty(item.name) ? item.name : "<None>")})").ToArray();
            int newIndex = EditorGUILayout.Popup("SpriteId", currentIndex, names);

            if (newIndex >= 0 && newIndex < symbols.Length) {
                m_spriteId.uintValue = symbols[newIndex].tagId;
            }

            serializedObject.ApplyModifiedProperties(); // 应用修改
        }
    }
}