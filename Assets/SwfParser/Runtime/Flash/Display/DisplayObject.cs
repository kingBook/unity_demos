using UnityEngine;

namespace SwfParserRuntime {

    public abstract class DisplayObject: MonoBehaviour {

        //
        public float alpha{ get; set; }
        //public filters
        //public DisplayObject mask;
        //public string name;
        public DisplayObjectContainer parent{ get; set; }
        public float rotation{ get; set; }
        public float scaleX{ get; set; }
        public float scaleY{ get; set; }
        //public Transform transform;
        public bool visible{ get; set; }
        public float width{ get; set; }
        public float height{ get; set; }
        public float x{ get; set; }
        public float y{ get; set; }
        
        public bool isInUI { get; protected set; }

        public DisplayObject() {
            alpha = 1;
            //transform = new Transform();
            visible = true;

            rotation = 0;
            scaleX = 1;
            scaleY = 1;
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