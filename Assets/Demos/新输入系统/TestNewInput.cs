using UnityEngine;
using UnityEngine.InputSystem;

public class TestNewInput : MonoBehaviour {
    void Start() {

    }

    void Update() {
        // 检查空格键是否在这个帧被按下  
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            Debug.Log("Space key was pressed");
        }
        // 检查空格键是否在这个帧被释放  
        if (Keyboard.current.spaceKey.wasReleasedThisFrame) {
            Debug.Log("Space key was released");
        }
        // 检查左鼠标键是否在这个帧被按下  
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Debug.Log("Left mouse button was pressed");
        }
        // 检查左鼠标键是否在这个帧被释放  
        if (Mouse.current.leftButton.wasReleasedThisFrame) {
            Debug.Log("Left mouse button was released");
        }
    }
}
