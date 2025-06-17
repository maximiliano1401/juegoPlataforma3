using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 5f;
    // public float attackCooldown = 0f;

    private Rigidbody2D Rigidbody2D;
    private CapsuleCollider2D capsuleCollider2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    // private float lastAttackTime = 0;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.green);
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-2f, 2f, 2f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(2f, 2f, 2f);

        Animator.SetBool("Correr", Horizontal != 0.0f);

        Grounded = IsGrounded();
        Animator.SetBool("Salto", !Grounded);

        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
            Animator.SetBool("Salto", true);
        }
        if (Input.GetKeyDown(KeyCode.S) && Grounded)
        {
            Duck();
            Animator.SetBool("Agacharse", true);
        }
        else if (Input.GetKeyUp(KeyCode.S) && Grounded)
        {
            Animator.SetBool("Agacharse", false);
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x, 0.35f);
            capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x, 0f);
        }

        // if (Input.GetKey(KeyCode.Space) && Time.time > lastAttackTime + attackCooldown)
        if (Input.GetKey(KeyCode.Space))
        {
            Animator.SetBool("Ataque", true);
            Animator.Play("chaAtaque");
            // Shoot();
            // lastAttackTime = Time.time;
        }
        else
        {
            Animator.SetBool("Ataque", false);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
        return hit.collider != null;
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Duck()
    {
        capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x, 0.15f);
        capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x, -0.08f);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
    }
}