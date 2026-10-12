using UnityEngine;
using UnityEngine.UI;

namespace SwfParserRuntime {

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
    public class MovieClipRenderer : MonoBehaviour {

        [SerializeField] private Swf m_swf;
        [SerializeField] private string m_symbolClassName;

        public MovieClip movieClip { get; private set; }

        private void Awake() {
            Debug.Log($"{GetType().Name}::Awake();");

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            movieClip = new MovieClip(m_swf, m_symbolClassName, isInUI: false);
        }

    }
}