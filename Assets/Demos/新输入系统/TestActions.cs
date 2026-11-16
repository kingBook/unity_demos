using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestActions : MonoBehaviour {

    public InputActionReference fireAction;
    public InputActionReference longJumpAction;
    public InputActionReference moveAction;

    [SerializeField]
    private TMP_Text _outputTxt;


    private void OnEnable() {
        fireAction.action.Enable();
        longJumpAction.action.Enable();
        moveAction.action.Enable();
    }

    private void OnDisable() {
        fireAction.action.Disable();
        longJumpAction.action.Disable();
        moveAction.action.Disable();
    }

    private void Update() {
        // 按下/释放
        if (fireAction.action.WasPressedThisFrame()) {
            Log("OnFireDown");
        }
        if (fireAction.action.WasReleasedThisFrame()) {
            Log("OnFireUp");
        }

        // 长按
        if (longJumpAction.action.WasPerformedThisFrame()) {
            Log("OnLongPress");
        }
        if (longJumpAction.action.WasReleasedThisFrame()) {
            Log("OnLongUp");
        }

        // 移动
        Vector2 moveVec = moveAction.action.ReadValue<Vector2>();
        if (moveVec.magnitude != 0.0f) {
            Log($"Move {moveVec}");
        }
    }

    private void Log(string msg) {
        Debug.Log(msg);
        _outputTxt.text = msg;
    }

}
