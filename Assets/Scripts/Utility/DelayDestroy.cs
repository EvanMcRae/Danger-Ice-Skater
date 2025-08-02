using UnityEngine;

namespace Utility {
    public class DelayDestroy : MonoBehaviour {
        public float destroyDelay;
        private float time;

        public void Start() {
            time = destroyDelay;
        }

        public void Update() {
            if (PauseManager.paused) return;

            destroyDelay -= Time.deltaTime;
            if (destroyDelay <= 0) {
                Destroy(gameObject);
            }
        }
        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && destroyDelay > 0)
            {
                Destroy(gameObject);
            }
        }
    }
}