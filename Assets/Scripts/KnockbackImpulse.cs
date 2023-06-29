using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackImpulse : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float strength = 20;
    [SerializeField]
    private float delay = 0.1f;

    [SerializeField]
    private UnityEvent onBeginKnockback;
    [SerializeField]
    private UnityEvent onEndKnockback;

    public void Knockback(Vector2 direction, float forceMultiplier = 1) {
        StopAllCoroutines();
        onBeginKnockback?.Invoke();
        rb2d.AddForce(direction * strength * forceMultiplier, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset() {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector2.zero;
        onEndKnockback?.Invoke();
    }
}
