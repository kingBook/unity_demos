using UnityEngine;
using UnityEngine.UI;

namespace SwfParserRuntime {

    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class MovieClipRendererUI : MonoBehaviour {

        [SerializeField] private Swf m_swf;
        [SerializeField] private string m_symbolClassName;
        
        public MovieClip movieClip { get; private set; }

        private void Awake() {
            Debug.Log($"{GetType().Name}::Awake();");
            //var meshHelper = new MeshHelperUI();
            movieClip = new MovieClip(m_swf, m_symbolClassName, isInUI: true);
        }



    }


}