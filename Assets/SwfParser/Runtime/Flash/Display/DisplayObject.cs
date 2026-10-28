using UnityEngine;

namespace SwfParserRuntime {

    public abstract class DisplayObject : MonoBehaviour {

        //

        //public filters
        //public DisplayObject mask;
        //public string name;
        public DisplayObjectContainer parent { get; set; }
        private TransformF m_transformF = new();
        private float m_rotation = 0.0f;
        private float m_scaleX = 1.0f;
        private float m_scaleY = 1.0f;
        private bool m_visible = true;
        private float m_alpha = 1.0f;
        private float m_x = 0.0f;
        private float m_y = 0.0f;
        public float width { get; set; }
        public float height { get; set; }

        public bool isInUI { get; protected set; }

        public TransformF transformF {
            get {
                return m_transformF;
            }
            set {
                
                m_transformF = value;
            }
        }

        public float rotation {
            get {
                return m_rotation;
            }
            set {
                m_rotation = value;
            }
        }

        public float scaleX {
            get {
                return m_scaleX;
            }
            set {
                m_scaleX = value;
            }
        }

        public float scaleY {
            get {
                return m_scaleY;
            }
            set {
                m_scaleY = value;
            }
        }

        public bool visible {
            get {
                return m_visible;
            }
            set {
                m_visible = value;
            }
        }

        public float alpha {
            get {
                return m_alpha;
            }
            set {
                m_alpha = value;
            }
        }

        public float x {
            get {
                return m_x;
            }
            set {
                m_x = value;
            }
        }

        public float y {
            get {
                return m_y;
            }
            set {
                m_y = value;
            }
        }

        protected virtual void Awake() {

        }

        protected virtual void OnEnable() {

        }

        protected virtual void Start() {

        }

        protected virtual void Update() {

        }

        // /// <summary>
        // /// 父节点变更
        // /// </summary>
        // protected virtual void OnTransformParentChanged() {
        //     // UI-RectTransform
        //     if (GetComponentInParent<Canvas>()) {
        //         return;
        //     }

        //     // Transform

        // }

        protected virtual void OnDisable() {

        }

        protected virtual void OnDestroy() {

        }

        public Rect GetBounds() {
            return new Rect();
        }

        public Rect GetRect() {
            return new Rect();
        }

        public Vector2 GlobalToLocal(Vector2 point) {
            return Vector2.zero;
        }

        public Vector2 LocalToGlobal(Vector2 point) {
            return Vector2.zero;
        }

    }
}