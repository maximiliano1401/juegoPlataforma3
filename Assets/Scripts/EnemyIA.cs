using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 1.5f;
    public float detectionRange = 1.3f;
    public float attackRange = 0.5f;
    public Transform[] patrolPoints;
    public Transform player;

    private int currentPatrolIndex = 0;
    private bool isChasing = false;
    public bool isAttacking = false;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            isChasing = false;
            Attack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isAttacking = false;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            isAttacking = false;
            Patrol();
        }

        UpdateAnimations();
    }

    void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }

    void Attack()
    {
        rb.linearVelocity = Vector2.zero; // Detener al enemigo mientras ataca
        Debug.Log("¡Ataque al jugador!");
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", !isAttacking && rb.linearVelocity.x != 0);
        animator.SetBool("isAttacking", isAttacking);

        if (rb.linearVelocity.x > 0) transform.localScale = new Vector3(2, 2, 2); // Mirando a la derecha
        else if (rb.linearVelocity.x < 0) transform.localScale = new Vector3(-2, 2, 2); // Mirando a la izquierda
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
