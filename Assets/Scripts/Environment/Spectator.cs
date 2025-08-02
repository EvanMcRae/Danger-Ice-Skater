using UnityEngine;

namespace Environment {
    public class Spectator : MonoBehaviour {

        public Vector3 startPos;

        public float amplitude;

        public bool excited = false;
        public bool setExcited = false;
        
        public float timer;
        public float timeScalar;
        
        public void Start() {
            startPos = transform.position;
            timer = 1;
        }
        public void Update() {
            timer = Mathf.Max(timer - Time.deltaTime * timeScalar, 0);
            if (timer <= 0) {
                timer = 1;
                excited = setExcited;
            }

            transform.position = startPos + new Vector3(0, amplitude * Mathf.Sin(timer * Mathf.PI * 2) + amplitude, 0);
        }

        public float ModAmplitude() {
            return amplitude * (excited ? 1 : 3);
        }
        
    }
}