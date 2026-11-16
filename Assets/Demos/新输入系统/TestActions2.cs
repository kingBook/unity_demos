using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestActions2 : MonoBehaviour {

    [SerializeField]
    private TMP_Text _outputTxt;

    private GameActions _gameActions;

    private void OnEnable() {
        _gameActions ??= new GameActions();

        // 按下/释放
        _gameActions.player.fire.started += OnFireDown;
        _gameActions.player.fire.canceled += OnFireUp;

        // 长按
        _gameActions.player.longJump.performed += OnLongPress;
        _gameActions.player.longJump.canceled += OnLongUp;

        // 启用
        _gameActions.Enable();
    }

    private void OnDisable() {
        // 按下/释放
        _gameActions.player.fire.started -= OnFireDown;
        _gameActions.player.fire.canceled -= OnFireUp;

        // 长按
        _gameActions.player.longJump.performed -= OnLongPress;
        _gameActions.player.longJump.canceled -= OnLongUp;

        // 禁用
        _gameActions.Disable();
    }

    private void Update() {
        // 移动
        Vector2 moveVec = _gameActions.player.move.ReadValue<Vector2>();
        if (moveVec.magnitude != 0.0f) {
            Log($"Move {moveVec}");
        }
    }

    private void Log(string msg) {
        Debug.Log(msg);
        _outputTxt.text = msg;
    }

    private void OnFireDown(InputAction.CallbackContext Obj) {
        Log("OnFireDown");
    }

    private void OnFireUp(InputAction.CallbackContext Obj) {
        Log("OnFireUp");
    }

    private void OnLongPress(InputAction.CallbackContext Obj) {
        Log("OnLongPress");
    }

    private void OnLongUp(InputAction.CallbackContext Obj) {
        Log("OnLongUp");
    }


}
