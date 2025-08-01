using Game;
using UI.PlayerUI;
using UnityEngine;

namespace Player {
    public class PlayerStatsHandler : MonoBehaviour {

        public HealthDisplay hd;

        public GameEventBroadcaster geb;
        
        
        public int health; //Int because it's a hits based health system.=
        public int maxHealth;

        public void Start() {
            health = maxHealth;
            hd.UpdateMaxHealth(maxHealth);
        }

        public bool Damage(int damage) {
            health = Mathf.Max(health - damage, 0);
            hd.DealDamage(damage);
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