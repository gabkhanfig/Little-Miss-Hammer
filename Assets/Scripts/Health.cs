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
    private float invulnerabilityTimeAfterDamage = 0.35f;

    [SerializeField, Tooltip("Arguments: Int32 = damage amount, Vector2 = damage direction, Boolean = is dead")] 
    private UnityEvent<int, Vector2, bool> onDamageEvent = null;

    [SerializeField, Tooltip("Arguments: Int32 = healing amount")] 
    private UnityEvent<int> onHealingEvent = null;

    [HideInInspector] 
    public bool dontDieFromFullHealth {get; private set; } = false;

    [HideInInspector, Tooltip("")] 
    public int healthToBeLeftWithFromFull {get; private set; } = 1;

    [HideInInspector, Tooltip("")] 
    public float fromFullInvulnerabilityTime {get; private set; } = 1.5f;


    public int GetMaxHealth() {return maxHealth;}
    public void _HealthEditorSetDontDieFromFullHealth(bool value) {dontDieFromFullHealth = value;}
    public void _HealthEditorSetHealthToBeLeftWithFromNull(int value) {healthToBeLeftWithFromFull = value;}
    public void _HealthEditorSetFromFullInvulnerabilityTime(float value) {fromFullInvulnerabilityTime = value;}

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
        if(dontDieFromFullHealth) {
            Debug.AssertFormat(healthToBeLeftWithFromFull < maxHealth, "Health script healthToBeLeftWithFromNull must be less than max health! Value is {0}", healthToBeLeftWithFromFull);
            Debug.AssertFormat(healthToBeLeftWithFromFull > 0, "Health script healthToBeLeftWithFromNull must be greater than 0! Value is {0}", healthToBeLeftWithFromFull);
            Debug.AssertFormat(fromFullInvulnerabilityTime > 0, "Health script fromFullInvulnerabilityTime must be greater than 0! Value is {0}", fromFullInvulnerabilityTime);
        }
        
        // For some reason doesn't update correctly when changing fromFullInvulnerabilityTime itself?
        fromFullInvulnerabilityTime = Mathf.Max(fromFullInvulnerabilityTime, invulnerabilityTimeAfterDamage);
    }

    public void Damage(int amount, Vector2 direction) {
        Debug.AssertFormat(amount > 0, "Damage amount must be a positive number. Amount is {0}", amount);
        direction.Normalize();
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

#if UNITY_EDITOR
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var health = target as Health;
        health._HealthEditorSetDontDieFromFullHealth(GUILayout.Toggle(health.dontDieFromFullHealth, "Dont Die From Full Health"));

        if(health.dontDieFromFullHealth) {
            health._HealthEditorSetHealthToBeLeftWithFromNull(EditorGUILayout.IntField("Health To Be Left With From Null", health.healthToBeLeftWithFromFull));
            health._HealthEditorSetFromFullInvulnerabilityTime(EditorGUILayout.FloatField("From Full Invulnerability Time", health.fromFullInvulnerabilityTime));
        }
    }
}
#endif
