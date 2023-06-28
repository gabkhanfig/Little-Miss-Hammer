using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField]
    private Health healthComponent;

    public Health GetHealthComponent() {return healthComponent;}

    //public Health healthComponent {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        healthComponent = gameObject.AddComponent(typeof(Health)) as Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
