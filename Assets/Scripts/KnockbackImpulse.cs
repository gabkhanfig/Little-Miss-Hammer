using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackImpulse : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float strength = 25;
    [SerializeField]
    private float delay = 0.05f;

    [SerializeField]
    private UnityEvent onBeginKnockback;
    [SerializeField]
    private UnityEvent onEndKnockback;

    public void Knockback(Vector2 direction) {
        StopAllCoroutines();
        onBeginKnockback?.Invoke();
        rb2d.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset() {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector2.zero;
        onEndKnockback?.Invoke();
    }
}
