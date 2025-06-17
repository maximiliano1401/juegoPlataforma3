using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public AudioSource shootSound;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Disparar con la barra espaciadora
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Determinar la dirección del disparo basado en la escala del jugador
        float direction = transform.localScale.x > 0 ? 1f : -1f; 
        shootSound.Play();

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(direction * projectileSpeed, 0); // Disparo en la dirección correcta

        // Ajustar la escala del proyectil para que no se invierta visualmente
        Vector3 newScale = projectile.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * direction; 
        projectile.transform.localScale = newScale;
    }
}
