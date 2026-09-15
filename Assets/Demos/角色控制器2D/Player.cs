using UnityEngine;

public class Player : MonoBehaviour {
    
    private CharacterController2D _characterController;
    
    private void Awake() {
        _characterController = GetComponent<CharacterController2D>();
    }
    
    private void Start() {
        
    }

    private void Update() {

    }
}
