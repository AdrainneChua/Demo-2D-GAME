using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IDamageable {
    public float Rightlimit = 0;
    public float Leftlimit = 0;
    
    public Transform MeleeAttackOrigin = null;
    public float MeleeAttackRadius = 0.6f;
    public float MeleeDamage = 2f;
    public float MeleeAttackDelay = 1.1f;
    public float HurtDelay = 1.1f;
    public float DeathDelay = 0.2f;
    public LayerMask layer = 8;

    private bool isMeleeAttacking = false;
    private bool isHurt = false;
    private bool isDeath = false;
    private bool facingLeft = true;

    void OnDrawGizmosSelected () {
        Debug.DrawRay (transform.position, -Vector2.up * groundedLeeway, Color.green);
        if (MeleeAttackOrigin != null) {
            Gizmos.DrawWireSphere (MeleeAttackOrigin.position, MeleeAttackRadius);
        }
    }

    public virtual void ApplyDamage (float Amount) {
        if (!isHurt) {
            Rb2D.velocity = Vector3.zero;
            StartCoroutine (HurtAnimDelay ());
            currentHealth -= Amount;
            if (currentHealth <= 0) {
                StartCoroutine (DeathAnimDelay ());
            }
        }
    }

    void Update () {
        HandleAI ();
    }

    private void HandleAI () {
        if (!isMeleeAttacking && !isHurt && !isDeath) {
            if (facingLeft) {
                if (transform.position.x > Leftlimit) {
                    Animator.SetInteger ("AnimState", 2);
                    Rb2D.velocity = new Vector2 (-speed, Rb2D.velocity.y);
                    transform.localScale = new Vector2 (1, 1);
                } else {
                    Rb2D.velocity = Vector3.zero;
                    HandleMeleeAttack ();
                    Animator.SetInteger ("AnimState", 0);
                    facingLeft = false;
                }
            } else {
                if (transform.position.x < Rightlimit) {
                    Animator.SetInteger ("AnimState", 2);
                    Rb2D.velocity = new Vector2 (speed, Rb2D.velocity.y);
                    transform.localScale = new Vector2 (-1, 1);
                } else {
                    Rb2D.velocity = Vector3.zero;
                    HandleMeleeAttack ();
                    Animator.SetInteger ("AnimState", 0);
                    Debug.Log ("Test");
                    facingLeft = true;
                }
            }
        }
    }

    private void HandleMeleeAttack () {
        if (!isMeleeAttacking) {
            Animator.SetTrigger ("Attack");
            StartCoroutine (MeleeAttackAnimDelay ());
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll (MeleeAttackOrigin.position, MeleeAttackRadius, layer);
            for (int i = 0; i < overlappedColliders.Length; i++) {
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable> ();
                if (enemyAttributes != null) {
                    enemyAttributes.ApplyDamage (MeleeDamage);
                }
            }
        }
    }

    private IEnumerator DeathAnimDelay () {
        Animator.SetTrigger ("Death");
        isDeath = true;
        yield return new WaitForSeconds (DeathDelay);
        Die ();
    }

    private IEnumerator HurtAnimDelay () {
        Animator.SetTrigger ("Hurt");
        isHurt = true;
        yield return new WaitForSeconds (HurtDelay);
        isHurt = false;
    }

    private IEnumerator MeleeAttackAnimDelay () {
        isMeleeAttacking = true;
        yield return new WaitForSeconds (MeleeAttackDelay);
        isMeleeAttacking = false;
    }
    
}