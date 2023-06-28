using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CurrentAttackAnim {
    Idle,
    Attack,
    Attack2

}

public class HammerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private CurrentAttackAnim attackAnimState = CurrentAttackAnim.Idle;
    private float timeSinceLastBasicAttack = 0;
    private float basicAttackDelayTime = 0;
    private bool canUseBasicAttack = true;

    private const float BASIC_ATTACK_ANIMATION_TIMEOUT = 0.5f;
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
        if(canUseBasicAttack && Input.GetMouseButton(0)) {
            BasicAttack();
            basicAttackDelayTime = 0;
            canUseBasicAttack = false;
        }

        basicAttackDelayTime += Time.deltaTime;
        if(basicAttackDelayTime >= BASIC_ATTACK_DELAY) {
            canUseBasicAttack = true;
            basicAttackDelayTime = 0;
        }

        else {
            if(timeSinceLastBasicAttack > BASIC_ATTACK_ANIMATION_TIMEOUT && attackAnimState != CurrentAttackAnim.Idle) {
                attackAnimState = CurrentAttackAnim.Idle;
                animator.SetTrigger("Idle");
            }
        }

        timeSinceLastBasicAttack += Time.deltaTime;


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
            attackAnimState = CurrentAttackAnim.Attack;
            break;
            case CurrentAttackAnim.Attack:
            animator.SetTrigger("Attack2");
            attackAnimState = CurrentAttackAnim.Attack2;
            break;
            case CurrentAttackAnim.Attack2:
            animator.SetTrigger("Attack");
            attackAnimState = CurrentAttackAnim.Attack;
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

            Debug.Log(collider.name);
        }
    }
}
