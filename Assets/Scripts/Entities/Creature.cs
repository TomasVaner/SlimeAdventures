using BaseClasses;
using Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Creature : NotifyMonoBehaviour
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
        private bool _canMove;
        
    #endregion
        
    #region Properties

        public bool CanMove
        {
            get => _canMove;
            private set => SetField(ref _canMove, value);
        }
        
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
            damagedTime = 0.0f;
        }

        public void Damage(float value, Vector3? source, float knockbackForce = 10)
        {
            if (damagedTime + immunityTime > Time.time)
                return;
            damagedTime = Time.time;
            currentHealth -= value;
            Debug.Log("Damaged for " + value + ", health left " + currentHealth);
            if (currentHealth <= 0.01f)
                Die();
            if (source.HasValue)
            {
                var knockback = (transform.position - source.Value).normalized;
                transform.Translate(knockback / 2);
                rb.AddForce(knockback * (knockbackForce / knockbackResistance), ForceMode2D.Impulse);
            }
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
