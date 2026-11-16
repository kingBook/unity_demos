using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour {

    public InputActionReference jumpAction;
    public InputActionReference moveAction;

    private CharacterController2D _ctrl;

    private void Awake() {
        _ctrl = GetComponent<CharacterController2D>();
    }

    private void OnEnable() {
        jumpAction.action.Enable();
        moveAction.action.Enable();
    }

    private void Start() {

    }

    private void Update() {
        // 跳
        if (jumpAction.action.WasPressedThisFrame()) {
            _ctrl.Jump();
        }

        // 左右移动
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        if (moveInput.magnitude != 0.0f) {
            _ctrl.Move(moveInput.x);
        }
    }

    private void OnDisable() {
        jumpAction.action.Disable();
        moveAction.action.Disable();
    }
}
