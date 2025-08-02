using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerUI {
    public class Heart : MonoBehaviour {
        public RectTransform rt;

        public Image image;
        public Sprite aliveHeart;
        public Sprite deadHeart;
        
        public bool alive = true;

        public void SetAlive(bool newAlive) {
            alive = newAlive;
            image.sprite = newAlive ? aliveHeart: deadHeart;
        }
    }
}