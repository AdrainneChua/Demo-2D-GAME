using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character, IDamageable {
    public KeyCode MeleeAttackKey = KeyCode.Mouse0;
    public KeyCode RangedAttackKey = KeyCode.Mouse1;
    public KeyCode jump = KeyCode.Space;
    public string xMoveaxis = "Horizontal";

    public Transform MeleeAttackOrigin = null;
    public float MeleeAttackRadius = 0.6f;
    public float MeleeDamage = 2f;
    public float MeleeAttackDelay = 1.1f;
    public LayerMask enemyLayer = 8;

    private float moveIntentionX = 0;
    private bool attemptJump = false;
    private bool attemptMeleeAttack = false;
    private float TimeUntilMeleeReadied = 0;
    private bool isMeleeAttacking = false;
    [SerializeField] private float hurtForce = 2f;

    [SerializeField] private int Coin = 0;
    [SerializeField] private Text CoinText;
    public Healthbar healthBar;
    
    void OnDrawGizmosSelected () {
        Debug.DrawRay (transform.position, -Vector2.up * groundedLeeway, Color.green);
        if (MeleeAttackOrigin != null) {
            Gizmos.DrawWireSphere (MeleeAttackOrigin.position, MeleeAttackRadius);
        }
    }

    void Start (){
        healthBar.SetMaxHealth(HealthPool);
    }

    // Update is called once per frame
    void Update () {
        GetInput ();
        HandleMeleeAttack ();
        HandleAnimation ();
        HandleJump ();
    }

    void FixedUpdate () {
        HandleRun ();
    }

    private void GetInput () {
        moveIntentionX = Input.GetAxis (xMoveaxis);
        attemptMeleeAttack = Input.GetKeyDown (MeleeAttackKey);
        attemptJump = Input.GetKeyDown (jump);
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "collectable") {
            Destroy (collision.gameObject);
            Coin += 1;
            CoinText.text = Coin.ToString ();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            
           
               ApplyDamage(hurtForce);

                if(collision.gameObject.transform.position.x > transform.position.x)
                {
                    //Move to left when damaged
                    Rb2D.velocity = new Vector2(-hurtForce, Rb2D.velocity.y);
                }
                else
                {
                    //Move to right when damaged
                    Rb2D.velocity = new Vector2(hurtForce, Rb2D.velocity.y);
                }
            
        }
    }

    private void HandleMeleeAttack () {
        if (attemptMeleeAttack && TimeUntilMeleeReadied <= 0) {
            Animator.SetTrigger ("Attack");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll (MeleeAttackOrigin.position, MeleeAttackRadius, enemyLayer);
            for (int i = 0; i < overlappedColliders.Length; i++) {
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable> ();
                if (enemyAttributes != null) {
                    enemyAttributes.ApplyDamage (MeleeDamage);
                }
            }
            TimeUntilMeleeReadied = MeleeAttackDelay;
        } else {
            TimeUntilMeleeReadied -= Time.deltaTime;
        }
    }

    private void HandleAnimation () {
        Animator.SetBool ("Grounded", CheckGrounded ());
        if (attemptMeleeAttack) {
            if (!isMeleeAttacking) {
                StartCoroutine (MeleeAttackAnimDelay ());
            }

        }
        if (attemptJump && CheckGrounded () || Rb2D.velocity.y > 1f) {
            if (!isMeleeAttacking) {
                Animator.SetTrigger ("Jump");
            }

        }
        if (Mathf.Abs (moveIntentionX) > 0.1f && CheckGrounded ()) {
            Animator.SetInteger ("AnimState", 2);
        } else {
            Animator.SetInteger ("AnimState", 0);
        }

    }

    private IEnumerator MeleeAttackAnimDelay () {
       
        isMeleeAttacking = true;
        yield return new WaitForSeconds (MeleeAttackDelay);
        isMeleeAttacking = false;
    }
    private void HandleJump () {
        if (attemptJump && CheckGrounded ()) {
            Rb2D.velocity = new Vector2 (Rb2D.velocity.x, jumpForce);
        }

    }

    private void HandleRun () {

        if (moveIntentionX > 0 && transform.rotation.y == 0 && !isMeleeAttacking) {
            Rb2D.velocity = new Vector2 (-speed, Rb2D.velocity.y);
            transform.localScale = new Vector2 (-1, 1);
        } else if (moveIntentionX < 0 && transform.rotation.y == 0 && !isMeleeAttacking) {
            Rb2D.velocity = new Vector2 (speed, Rb2D.velocity.y);
            transform.localScale = new Vector2 (1, 1);
        }
        Rb2D.velocity = new Vector2 (moveIntentionX * speed, Rb2D.velocity.y);
    }
    
    public virtual void ApplyDamage (float Amount) {
        currentHealth -= Amount;
        healthBar.SetHealth(currentHealth);
        Animator.SetTrigger ("Hurt");
        if (currentHealth <= 0) {
            Die ();
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
    }

}