using UnityEngine;

namespace Utility {
    public class DelayDestroy : MonoBehaviour {
        public float destroyDelay;
        private float time;

        public void Start() {
            time = destroyDelay;
        }

        public void Update() {
            destroyDelay -= Time.deltaTime;
            if (destroyDelay <= 0) {
                Destroy(gameObject);
            }
        }
    }
}