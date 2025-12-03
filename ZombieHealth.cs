using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;
    
    // Properties required by the Alteruna Gun script
    public float PenetrationResistance = 5f; 
    public int MaterialType = 0; 

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(ushort senderID, float amount)
    {
        currentHealth -= amount;
        // Debug.Log($"Zombie Hit! HP: {currentHealth}");

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