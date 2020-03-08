using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float HealthPool = 10f;
    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundedLeeway = 0.1f;

    private Rigidbody2D  rb2D = null;
    private float CurrentHealth = 10f;
    private Animator animator =null;
    public LayerMask ground;

    public Rigidbody2D Rb2D
    {
        get {return rb2D;}
        protected set{rb2D = value;}
    }

     public float currentHealth
    {
        get {return CurrentHealth;}
        protected set{CurrentHealth = value;}
    }

    public Animator Animator{
        get {return animator;}
        protected set{animator = value;}
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(GetComponent<Rigidbody2D>()){
            rb2D = GetComponent<Rigidbody2D>();
        }
        if(GetComponent<Animator>()){
            animator = GetComponent<Animator>();
        }
        CurrentHealth = HealthPool;
    }

     protected bool CheckGrounded(){
        return Physics2D.Raycast(transform.position,-Vector2.up,groundedLeeway,ground);
    }
    
     protected virtual void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
