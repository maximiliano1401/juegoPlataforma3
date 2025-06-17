using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    public int damage = 1;
    public int scoreValue = 10;
    public string deathAnimationName = "Muerte1"; // Nombre configurable de la animación de muerte
    private EnemyAI enemyAI;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    public AudioSource muerteSonido;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("El enemigo ha muerto.");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.GetComponent<PlayerHealth>().AddScore(scoreValue);
        }

        isDead = true;

        // Forzar la animación de muerte
        animator.Play(deathAnimationName); // Usar el nombre configurable de la animación
        muerteSonido.Play();
        // Detener movimiento y desactivar IA
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.EnemyDefeated();
        }

        // Destruir después del tiempo de la animación
        float deathAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deathAnimationDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            if (enemyAI != null && enemyAI.isAttacking)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }
}
