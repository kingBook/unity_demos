using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    public float slopeLimit = 45f;
    public float stepOffset = 0.3f;
    public float skinWidth = 0.08f;
    public float minMoveDistance = 0.001f;
    public Vector3 center;
    public float radius = 0.5f;
    public float height = 2;


    private Rigidbody2D _body;
    private BoxCollider2D _collider;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }


}
