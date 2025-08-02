using Game;
using UI.PlayerUI;
using UnityEngine;

namespace Player {
    public class PlayerStatsHandler : MonoBehaviour {

        public HealthDisplay hd;

        public GameEventBroadcaster geb;

        public MeshRenderer mr;

        public float invincibilityTimer;
        public float invincibilityTime;
        
        public int health; //Int because it's a hits based health system.=
        public int maxHealth;

        public void Start() {
            health = maxHealth;
            hd.UpdateMaxHealth(maxHealth);
        }

        public void Update() {
            invincibilityTimer = Mathf.Max(invincibilityTimer - Time.deltaTime, 0);
            if (invincibilityTimer > 0) {
                int temp = (int) (invincibilityTimer * 10);
                mr.enabled = temp % 2 == 0;
            }
            else {
                mr.enabled = true;
            }
        }

        public bool Damage(int damage) {
            if (invincibilityTimer > 0) return false;
            health = Mathf.Max(health - damage, 0);
            hd.DealDamage(damage);
            if (damage > 0) invincibilityTimer = invincibilityTime;
            if (health == 0) KillFromDamage();
            return health == 0;
        }
        
        public void Heal(int amount) {
            health = Mathf.Min(health + amount, maxHealth);
            hd.Heal(amount);
        }

        public void KillFromDamage() {
            geb.OnPlayerDeathByHit.Invoke(this);
        }
    }
}