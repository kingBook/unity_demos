using UnityEngine;
using UnityEngine.UI;

namespace SwfParserRuntime {

    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class ClipRendererUI : MonoBehaviour {

        [SerializeField] private Swf m_swf;
        [SerializeField] private string m_symbolClassName;
        
        public MovieClip movieClip { get; private set; }

        private void Awake() {
            Debug.Log($"{GetType().Name}::Awake();");
            movieClip = new MovieClip(m_swf, m_symbolClassName, isInUI: true);
        }



    }


}