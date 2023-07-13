using System;
using Controller;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Creature : MonoBehaviour
    {
    #region Variables
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private float immunityTime;
        [SerializeField] private float knockbackResistance;
        [SerializeField] private float maxHealth;
    #endregion

    #region Private Fields

        private Rigidbody2D rb;
        private SpriteAnimationController anim;
        private float currentHealth;
        private float damagedTime;

    #endregion
        
    #region Properties
        
        public bool CanMove { get; private set; }
        
    #endregion
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<SpriteAnimationController>();
        }
        
        private void Start()
        {
            Respawn();
        }
        
        private void Respawn()
        {
            transform.position = spawnPosition.position;
            anim.SetDying(false);
            CanMove = true;
            currentHealth = maxHealth;
        }

        public void Damage(float value, Transform source, float knockbackForce = 10)
        {
            if (damagedTime + immunityTime < Time.time)
                return;
            damagedTime = Time.time;
            currentHealth -= value;
            if (currentHealth < 0f)
                Die();
            rb.velocity = (source.transform.position - transform.position).normalized *
                          (knockbackForce / knockbackResistance);
        }
        
        private void Die()
        {
            anim.SetDying(true);
            CanMove = false;
        }

        public void Died()
        {
            Respawn();
        }
    }
}
