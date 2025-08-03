using System;
using Game;
using UI.PlayerUI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player {
    public class PlayerStatsHandler : MonoBehaviour {

        public HealthDisplay hd;

        public GameEventBroadcaster geb;

        public float invincibilityTimer;
        public float invincibilityTime;
        
        public int health; //Int because it's a hits based health system.=
        public int maxHealth;

        public int waveClearHealAmount;

        private SkinnedMeshRenderer[] meshes;

        [SerializeField]
        public Animator anim;

        public bool killed = false;

        public void Start() {
            health = maxHealth;
            hd.UpdateMaxHealth(maxHealth);
            meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        public void Update() {
            invincibilityTimer = Mathf.Max(invincibilityTimer - Time.deltaTime, 0);
            if (invincibilityTimer > 0) {
                int temp = (int) (invincibilityTimer * 10);
                bool active = temp % 2 == 0;
                foreach (SkinnedMeshRenderer mesh in meshes) {
                    mesh.renderingLayerMask = active ? 1u : 0u;
                }
            }
            else {
                foreach (SkinnedMeshRenderer mesh in meshes) {
                    mesh.renderingLayerMask = 1u;
                }
            }
        }

        public bool Damage(int damage) {
            if (invincibilityTimer > 0) return false;
            health = Mathf.Max(health - damage, 0);
            hd.DealDamage(damage);
            anim.SetTrigger("damaged");
            if (damage > 0 && health != 0) invincibilityTimer = invincibilityTime;
            if (health == 0 && !killed) StartCoroutine(KillFromDamage());
            //KillFromDamage();
            
            return health == 0;
        }
        
        public void Heal(int amount) {
            health = Mathf.Min(health + amount, maxHealth);
            hd.Heal(health);
        }

        //public void KillFromDamage() {
        //    anim.SetTrigger("die");
        //    geb.OnPlayerDeathByHit.Invoke(this);
        //}

        public IEnumerator KillFromDamage()
        {
            anim.SetTrigger("die");
            if (!killed)
            {
                AkUnitySoundEngine.PostEvent("PlayerDeath", gameObject);
            }
            killed = true;

            // Optional delay before invoking the event
            yield return new WaitForSeconds(1.5f); // adjust as needed

            geb.OnPlayerDeathByHit.Invoke(this);
        }

        public void OnEnable() {
            geb.OnWaveClear.AddListener(OnWaveClear);
        }

        public void OnDisable() {
            geb.OnWaveClear.RemoveListener(OnWaveClear);
        }

        public void OnWaveClear(int wave) {
            if (WinLoseUI.lostGame) return;
            Heal(waveClearHealAmount);
        }
    }
}