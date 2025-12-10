using UnityEngine;


    public class ZombieHealth : MonoBehaviour
    {
        public float maxHealth = 50f;
        private float currentHealth;

        public float PenetrationResistance = 5f;
        public int MaterialType = 0;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log($"Zombie Hit! HP: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }

