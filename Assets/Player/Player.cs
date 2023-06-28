using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [HideInInspector] public Health healthComponent {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        healthComponent = GetComponent<Health>();
        Debug.Assert(healthComponent != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
