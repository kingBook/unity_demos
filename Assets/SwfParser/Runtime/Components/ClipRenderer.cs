using UnityEngine;
using UnityEngine.UI;

namespace SwfParserRuntime {

    [DisallowMultipleComponent]
    public class ClipRenderer : MonoBehaviour {

        [SerializeField] private Swf m_swf;
        [SerializeField] private string m_symbolClassName;

        public MovieClip movieClip { get; private set; }

        private void Awake() {
            Debug.Log($"{GetType().Name}::Awake();");


            movieClip = new MovieClip(m_swf, m_symbolClassName, isInUI: false);
        }

    }
}