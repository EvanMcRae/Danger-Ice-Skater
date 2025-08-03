using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerUI {
    public class Heart : MonoBehaviour {
        public RectTransform rt;

        public Image image;
        public Sprite aliveHeart;
        public Sprite deadHeart;
        
        public bool alive = true;

        public float timer;
        
        public void SetAlive(bool newAlive) {
            alive = newAlive;
            image.sprite = newAlive ? aliveHeart: deadHeart;
            timer = 1;
        }

        public void Update() {
            timer -= Time.deltaTime;

            if (timer > 0) {
                timer -= Time.deltaTime;
                image.sprite = (int) (timer * 10) % 2 == 0 ? aliveHeart : deadHeart;
            }
            else {
                image.sprite = alive ? aliveHeart: deadHeart;
            }
        }
    }
}