using UnityEditor;
using UnityEngine;
using System.Linq;

namespace SwfParserRuntime {

    [CustomEditor(typeof(MovieClip))]
    public class MovieClipEditor : Editor {

        /// <summary> 序列化属性 m_swf </summary>
        private SerializedProperty m_swf;

        /// <summary> 序列化属性 m_symbolClassName </summary>
        private SerializedProperty m_symbolClassName;

        private void OnEnable() {
            m_swf = serializedObject.FindProperty("swf");
            m_symbolClassName = serializedObject.FindProperty("symbolClassName");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update(); // 更新序列化对象

            // swf 属性
            EditorGUILayout.PropertyField(m_swf);


            // symbolClassName 属性
            MovieClip mc = (MovieClip)target;
            if (mc.swf == null) {
                serializedObject.ApplyModifiedProperties();  // 应用修改
                return;
            }

            SymbolClassRecord[] symbols = mc.swf.GetSymbols();
            if (symbols == null || symbols.Length == 0) {
                EditorGUILayout.HelpBox("没有可用的 Symbol", MessageType.Info);
                serializedObject.ApplyModifiedProperties();  // 应用修改
                return;
            }

            int currentIndex = System.Array.FindIndex(symbols, item => item.name == m_symbolClassName.stringValue);
            if (currentIndex < 0) currentIndex = 0;

            // string[] names = symbols.Select(item => $"{(!string.IsNullOrEmpty(item.name) ? item.name : "<None>")} ({item.tagId})").ToArray();
            string[] names = symbols.Select(item => $"{(!string.IsNullOrEmpty(item.name) ? item.name : "<None>")}").ToArray();
            int newIndex = EditorGUILayout.Popup("SymbolClassName", currentIndex, names);

            if (newIndex >= 0 && newIndex < symbols.Length) {
                m_symbolClassName.stringValue = symbols[newIndex].name;
            }

            serializedObject.ApplyModifiedProperties(); // 应用修改
        }
    }
}