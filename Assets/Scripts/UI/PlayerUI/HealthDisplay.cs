using System.Collections.Generic;
using Player;
using UnityEngine;

namespace UI.PlayerUI {
    public class HealthDisplay : MonoBehaviour {

        public PlayerStatsHandler psh;

        public RectTransform rt;
        public Heart heart;
        public List<Heart> heartImages;

        public void DealDamage(int dmg) {
            for (int i = heartImages.Count - 1; i >= 0; i--) {
                if (heartImages[i].alive) {
                    heartImages[i].SetAlive(false);
                    dmg--;
                }

                if (dmg == 0) break;
            }

            if (!heartImages[0].alive) {
                Debug.Log("UI thinks we're dead!");
            }
        }

        public void Heal(int amount) {
            Debug.Log(amount);
            for (int i = 0; i < amount; i++) {
                if (!heartImages[i].alive) {
                    heartImages[i].SetAlive(true);
                }
            }
        }

        public void UpdateMaxHealth(int newMaxHealth) {
            int oldMaxHealth = heartImages.Count;
        
            if (newMaxHealth > oldMaxHealth) { //Adding hearts
                for (int i = oldMaxHealth; i < newMaxHealth; i++) {
                    CreateHeart(i);
                }
            }
            else if (oldMaxHealth > newMaxHealth) { //Removing hearts
                for (int i = 0; i < (oldMaxHealth - newMaxHealth); i++) {
                    RemoveLastHeart();
                }
            }
            
            rt.sizeDelta = new Vector2(55 * newMaxHealth + 25, 100);
            
        }

        public void CreateHeart(int index) {
            
            Heart h = Instantiate(heart, transform);
            h.rt.anchoredPosition = new Vector2(15 + 55 * index, 0);
            heartImages.Add(h);
        }

        public void RemoveLastHeart() {
            Destroy(heartImages[^1]);
            heartImages.RemoveAt(heartImages.Count - 1);
        }
    }
}