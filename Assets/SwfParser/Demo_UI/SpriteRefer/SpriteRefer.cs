using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRefer : MonoBehaviour {

    [SerializeField]
    private Sprite _sprite;

    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _spriteRenderer.sprite = _sprite;
    }
}
