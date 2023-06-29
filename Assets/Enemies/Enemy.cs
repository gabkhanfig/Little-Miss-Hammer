using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour, IEnemy
{
    private Health healthComponent;
    private KnockbackImpulse knockbackComponent;
    private Rigidbody2D rb2d;
    
    // Start is called before the first frame update
    void Start()
    {
        healthComponent = GetComponent<Health>();
        Debug.Assert(healthComponent != null, "Enemy must have a health component");
        rb2d = GetComponent<Rigidbody2D>();
        Debug.Assert(rb2d != null, "Enemy must have a rigidbody2d component");
        knockbackComponent = GetComponent<KnockbackImpulse>(); // Can be null
    }

    // Update is called once per frame
    void Update()
    {
    }

#region IEnemy 

    public Health GetHealthComponent() {return healthComponent;}

    public void OnDamageEvent(int damageAmount, Vector2 damageDirection, bool isDead) {
        Debug.LogFormat("{0} health remaining", healthComponent.currentHealth);
        knockbackComponent?.Knockback(damageDirection);
    }

    public void OnHealingEvent(int healingAmount) {

    }

#endregion IEnemy
}
