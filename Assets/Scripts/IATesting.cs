using Input;
using UnityEngine;

public class IATesting : MonoBehaviour {
    public InputManager imso;
    public void Update() {
        Debug.Log("XAXis ref: " + imso.xAxis.action.ReadValue<float>());
    }
}