using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [Tooltip("Measured in seconds")]
    [SerializeField] private float invulnerabilityTimeAfterDamage = 0.5f;
    [Tooltip("Arguments: Int32 = damage amount, Vector2 = damage direction, Boolean = is dead")]
    [SerializeField] private UnityEvent<int, Vector2, bool> onDamageEvent = null;
    [Tooltip("Arguments: Int32 = healing amount")]
    [SerializeField] private UnityEvent<int> onHealingEvent = null;
    [HideInInspector] public int currentHealth { get; private set; } = 0;

    public int GetMaxHealth() {return maxHealth;}

    // Start is called before the first frame update
    void Start()
    {
        Debug.AssertFormat(maxHealth > 0, "Health script maxHealth must be greater than 0 on start. Value is {0}", maxHealth);
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void OnValidate() {
        Debug.AssertFormat(maxHealth > 0, "Health script maxHealth must be greater than 0! Value is {0}", maxHealth);
    }

    public void Damage(int amount, Vector2 direction) {
        Debug.AssertFormat(amount > 0, "Damage amount must be a positive number. Amount is {0}", amount);
        if(currentHealth <= 0) return;

        currentHealth -= amount;
        bool isDead = currentHealth <= 0;
        if(isDead)
            currentHealth = 0;

        onDamageEvent?.Invoke(amount, direction, isDead);
    }

    public void Heal(int amount) {
        Debug.AssertFormat(amount > 0, "Heal amount must be a positive number. Amount is {0}", amount);
        if(currentHealth <= 0) return;

        int amountHealed = amount;
        if((currentHealth + amount) > maxHealth) {
            currentHealth = maxHealth;
            amountHealed = maxHealth - currentHealth;
        }
        else {
            currentHealth += amount;
        }

        onHealingEvent?.Invoke(amountHealed);
    }
}
