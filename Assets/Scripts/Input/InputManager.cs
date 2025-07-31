using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Input {
    
    [CreateAssetMenu(fileName = "InputManager")]
    public class InputManager : ScriptableObject {
        public InputActionReference xAxis;
        public InputActionReference yAxis;
        public InputActionReference jump;
    }
}
