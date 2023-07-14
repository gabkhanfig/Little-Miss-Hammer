using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IEnemy
{
    private Health healthComponent;

    [SerializeField]
    private float timeBetweenSpawns;

    private float timeUntilNextSpawn = 0;
    private bool isDoingSpawn = false;

    public Health GetHealthComponent() {return healthComponent;}


    // Start is called before the first frame update
    void Start()
    {
        healthComponent = GetComponent<Health>();
        Debug.Assert(healthComponent != null, "Enemy must have a health component");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDoingSpawn && timeUntilNextSpawn == 0) {
            Debug.Log("spawn!");
            timeUntilNextSpawn = timeBetweenSpawns;
        }
    }

    public void OnDamageEvent(int damageAmount, Vector2 damageDirection, bool isDead) {
        //Debug.LogFormat("{0} health remaining", healthComponent.currentHealth);
        //float knockbackMultiplier = isDead ? 4 : 1;
        //knockbackComponent?.Knockback(damageDirection, knockbackMultiplier);
    }

    public void OnHealingEvent(int healingAmount) {

    }
}
