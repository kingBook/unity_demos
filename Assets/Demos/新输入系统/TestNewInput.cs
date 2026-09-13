using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNewInput : MonoBehaviour {

    [SerializeField]
    private TMP_Text _outputTxt;

    void Start() {

    }

    void Update() {
        // 检查空格键是否在这个帧被按下  
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            Log("Space key was pressed");
        }
        // 检查空格键是否在这个帧被释放  
        if (Keyboard.current.spaceKey.wasReleasedThisFrame) {
            Log("Space key was released");
        }
        // 检查左鼠标键是否在这个帧被按下  
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Log("Left mouse button was pressed");
        }
        // 检查左鼠标键是否在这个帧被释放  
        if (Mouse.current.leftButton.wasReleasedThisFrame) {
            Log("Left mouse button was released");
        }
    }

    private void Log(string msg) {
        Debug.Log(msg);
        _outputTxt.text = msg;
    }
}
