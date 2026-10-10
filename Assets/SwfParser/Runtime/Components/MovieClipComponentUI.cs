using UnityEngine;
using UnityEngine.UI;

namespace SwfParserRuntime {

    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class MovieClipComponentUI : MonoBehaviour {

        [SerializeField] private Swf m_swf;
        [SerializeField] private string m_symbolClassName;
        private MovieClip m_movieClip;

        private void Awake() {
            Debug.Log($"{GetType().Name}::Awake();");
            var meshHelper = new MeshHelperUI();
            m_movieClip = new MovieClip(m_swf, meshHelper, m_symbolClassName, isInUI: true);
        }



    }


}