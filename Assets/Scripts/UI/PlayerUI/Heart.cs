using UnityEngine;

namespace UI.PlayerUI {
    public class Heart : MonoBehaviour {
        public RectTransform rt;

        public bool alive = true;

        public void SetAlive(bool newAlive) {
            alive = newAlive;
            gameObject.SetActive(alive);
        }
    }
}