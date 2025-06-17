using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 5f;
    public int daño = 1;
    public float tiempoDeVida = 3f;
    private float direccion = 1;
    private Rigidbody2D rb;

    public void SetDireccion(float nuevaDireccion)
    {
        direccion = nuevaDireccion;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ajustar la velocidad según la dirección
        rb.linearVelocity = new Vector2(velocidad * direccion, 0);

        // Ajustar la escala para que se vea en la dirección correcta
        transform.localScale = new Vector3(direccion * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, tiempoDeVida);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerHealth>().TakeDamage(daño);
            Destroy(gameObject);
        }
        else if (col.CompareTag("Ground") || col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
