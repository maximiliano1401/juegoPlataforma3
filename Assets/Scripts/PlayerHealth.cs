using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI livesTextTMP;
    public TextMeshProUGUI scoreTextTMP;
    public TextMeshProUGUI gameOverTextTMP; // Referencia al texto de Game Over
    public Transform respawnPosition;
    private int lives;
    private int score;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lives = GameManager.Instance.playerLives;
        score = GameManager.Instance.playerScore;
        UpdateUI();

        // Asegurarse de que el texto de Game Over esté desactivado al inicio
        if (gameOverTextTMP != null)
        {
            gameOverTextTMP.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        lives -= damage;
        GameManager.Instance.UpdateLives(-damage);
        UpdateUI();

        if (lives <= 0)
        {
            Die();
        }
        else
        {
            Respawn();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        animator.Play("MuerteJugador");

        Debug.Log("¡El jugador ha muerto!");

        // Mostrar el texto de Game Over
        if (gameOverTextTMP != null)
        {
            gameOverTextTMP.gameObject.SetActive(true);
        }

        // Desactivar colisiones con todo excepto objetos etiquetados como "Ground"
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Ground"))
            {
                collider.isTrigger = true;
            }
        }

        // Esperar el tiempo de la animación antes de desactivar el objeto
        float deathAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deathAnimationDuration);
    }

    private void Respawn()
    {
        Debug.Log("Jugador ha perdido una vida. Regresando a la posición inicial...");
        rb.linearVelocity = Vector2.zero; // Detener el movimiento
        transform.position = respawnPosition.position; // Teletransportar al punto de respawn
    }

    public void AddScore(int points)
    {
        score += points;
        GameManager.Instance.UpdateScore(points);
        UpdateUI();
    }

    private void UpdateUI()
    {
        livesTextTMP.text = "Vidas: " + lives;
        scoreTextTMP.text = "Puntaje: " + score;
    }
}
