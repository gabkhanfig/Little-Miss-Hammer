using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentAttackAnim {
    Idle,
    Attack,
    Idle2,
    Attack2

}

public class HammerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private CurrentAttackAnim attackAnimState = CurrentAttackAnim.Idle;
    private float timeSinceLastBasicAttack = 0;
    private float basicAttackDelayTime = 0;
    private bool canUseBasicAttack = true;
    private const float BASIC_ATTACK_DELAY = 0.35f;

    [SerializeField]
    private Transform damageOrigin;

    [SerializeField]
    private float damageRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsAttacking() && Input.GetMouseButton(0)) {
            BasicAttack();
            basicAttackDelayTime = 0;
            canUseBasicAttack = false;
        }

        basicAttackDelayTime += Time.deltaTime;
        if(basicAttackDelayTime >= BASIC_ATTACK_DELAY) {
            canUseBasicAttack = true;
            basicAttackDelayTime = 0;
        }

        timeSinceLastBasicAttack += Time.deltaTime;


    }

    public void SetAttackAnimState(CurrentAttackAnim newAnimState) {
        Debug.Log(newAnimState.ToString());
        attackAnimState = newAnimState;
    }

    private bool IsAttacking() {
        return attackAnimState == CurrentAttackAnim.Attack || attackAnimState == CurrentAttackAnim.Attack2;
    }

    private void BasicAttack() {
        timeSinceLastBasicAttack = 0;
        UpdateBasicAttackAnimation();
        DetectColliders();
    }

    private void UpdateBasicAttackAnimation() {
        switch(attackAnimState) {
            case CurrentAttackAnim.Idle:
            animator.SetTrigger("Attack");
            break;
            case CurrentAttackAnim.Idle2:
            animator.SetTrigger("Attack2");
            break;
        }
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Vector3 position = damageOrigin == null ? Vector3.zero : damageOrigin.position;
        Gizmos.DrawWireSphere(position, damageRadius);
    }

    public void DetectColliders() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(damageOrigin.position, damageRadius);
        foreach (Collider2D collider in colliders) {
            IEnemy enemyComponent = collider.GetComponent(typeof(IEnemy)) as IEnemy;
            if(enemyComponent == null) 
                continue;

            Health enemyHealth = enemyComponent.GetHealthComponent();
            Debug.Assert(enemyHealth != null, "Enemy must have a health component");

            Vector2 damageDirection = ( new Vector2(collider.transform.position.x, collider.transform.position.y) - new Vector2(damageOrigin.position.x, damageOrigin.position.y)).normalized;
            enemyHealth.Damage(1, damageDirection);
        }
    }
}
