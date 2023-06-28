using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private int maxHealth = 1;

    [HideInInspector] 
    public int currentHealth { get; private set; } = 0;

    [SerializeField, Tooltip("Measured in seconds")] 
    private float afterDamageInvulnerabilityTime = 0.35f;

    [SerializeField, Tooltip("Arguments: Int32 = damage amount, Vector2 = damage direction, Boolean = is dead")] 
    private UnityEvent<int, Vector2, bool> onDamageEvent = null;

    [SerializeField, Tooltip("Arguments: Int32 = healing amount")] 
    private UnityEvent<int> onHealingEvent = null;

    [HideInInspector, Tooltip("If set to true, this health component's owner will survive a killing blow from full health with the 1 health, and set to be invulnerable for fromFullInvulnerabilityTime")] 
    public bool dontDieFromFullHealth {get; private set; } = false;

    [HideInInspector, Tooltip("The duration of invulnerability after surviving an attack from full health. This value will always be greater than or equal to invulnerabilityTimeAfterDamage")] 
    public float fromFullInvulnerabilityTime {get; private set; } = 1.5f;

    [HideInInspector]
    public bool isInvulnerable {get; private set; } = false;
    private float invulnerabilityTimeRemaining = 0;


    public int GetMaxHealth() {return maxHealth;}
    public void _HealthEditorSetDontDieFromFullHealth(bool value) {dontDieFromFullHealth = value;}
    public void _HealthEditorSetFromFullInvulnerabilityTime(float value) {
        fromFullInvulnerabilityTime = Mathf.Max(value, afterDamageInvulnerabilityTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.AssertFormat(maxHealth > 0, "Health script maxHealth must be greater than 0 on start. Value is {0}", maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(invulnerabilityTimeRemaining > 0) {
            invulnerabilityTimeRemaining -= Time.deltaTime;
            if(invulnerabilityTimeRemaining <= 0) {
                invulnerabilityTimeRemaining = 0;
                isInvulnerable = false;
            }
        }
    }

    private void OnValidate() {
        Debug.AssertFormat(maxHealth > 0, "Health script maxHealth must be greater than 0! Value is {0}", maxHealth);
        if(dontDieFromFullHealth) {
            Debug.AssertFormat(fromFullInvulnerabilityTime > 0, "Health script fromFullInvulnerabilityTime must be greater than 0! Value is {0}", fromFullInvulnerabilityTime);
        }
    }

    public void Damage(int amount, Vector2 direction) {
        Debug.AssertFormat(amount > 0, "Damage amount must be a positive number. Amount is {0}", amount);
        direction.Normalize();
        if(currentHealth <= 0) return;

        bool isFullHealth = currentHealth == maxHealth;
        bool isKillingBlow = (currentHealth - amount) <= 0;
        if(dontDieFromFullHealth && isFullHealth && isKillingBlow) {
            int amountDealt = maxHealth - 1;
            currentHealth = 1;

            onDamageEvent?.Invoke(amountDealt, direction, false);
            SetTemporaryInvulnerability(fromFullInvulnerabilityTime);
            return;
        }

        currentHealth -= amount;
        bool isDead = currentHealth <= 0;
        if(isDead)
            currentHealth = 0;

        onDamageEvent?.Invoke(amount, direction, isDead);
        SetTemporaryInvulnerability(afterDamageInvulnerabilityTime);
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

    public void SetTemporaryInvulnerability(float duration) {
        invulnerabilityTimeRemaining = Mathf.Max(invulnerabilityTimeRemaining, duration);
        isInvulnerable = true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var health = target as Health;
        health._HealthEditorSetDontDieFromFullHealth(GUILayout.Toggle(health.dontDieFromFullHealth, "Dont Die From Full Health"));

        if(health.dontDieFromFullHealth) {
            health._HealthEditorSetFromFullInvulnerabilityTime(EditorGUILayout.FloatField("From Full Invulnerability Time", health.fromFullInvulnerabilityTime));
        }
    }
}
#endif
