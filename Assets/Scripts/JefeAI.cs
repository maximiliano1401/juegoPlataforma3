using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI
using System.Collections;

public class JefeAI : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadMovimiento = 3f;
    public GameObject proyectilPrefab;
    public Transform puntoDisparo1; // Primer punto de disparo
    public Transform puntoDisparo2; // Segundo punto de disparo
    public float tiempoEntreAtaques = 3f;
    public float tiempoEntreDisparos = 0.5f;
    public Slider healthBar; // Referencia a la barra de vida

    private Enemy enemy; // Referencia al script Enemy
    private Animator animator;
    private Rigidbody2D rb;
    private bool moviendoAHaciaB = true;
    private bool estaAtacando = false;
    private bool estaMuerto = false;
    public int damage = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Obtener referencia al script Enemy
        enemy = GetComponent<Enemy>();

        // Configurar la barra de vida
        if (healthBar != null && enemy != null)
        {
            healthBar.maxValue = enemy.health; // Establecer el valor máximo de la barra
            healthBar.value = enemy.health;    // Inicializar la barra con la vida completa
            Debug.Log($"Barra de vida inicializada: MaxValue = {healthBar.maxValue}, Value = {healthBar.value}");
        }

        StartCoroutine(PatrullarYAtacar());
    }

    void Update()
    {
        if (healthBar != null && enemy != null)
        {
            healthBar.value = enemy.health;
        }
    }

    IEnumerator PatrullarYAtacar()
    {
        while (!estaMuerto)
        {
            yield return Quieto();
            yield return AtaqueMovimiento();
            yield return AtaqueDisparo();
        }
    }

    IEnumerator Quieto()
    {
        animator.SetBool("isIdle", true);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(tiempoEntreAtaques);
        animator.SetBool("isIdle", false);
    }

    IEnumerator AtaqueMovimiento()
    {
        animator.SetTrigger("AtaqueMovimiento");

        Transform objetivo = moviendoAHaciaB ? puntoB : puntoA;
        Flip(objetivo.position.x < transform.position.x);

        while (Vector2.Distance(transform.position, objetivo.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        // Al llegar al objetivo, voltear según corresponda
        if (moviendoAHaciaB)
        {
            Flip(true); // Llegó a puntoB, debe mirar a la izquierda
        }
        else
        {
            Flip(false); // Llegó a puntoA, debe mirar a la derecha
        }

        moviendoAHaciaB = !moviendoAHaciaB;
    }

    IEnumerator AtaqueDisparo()
    {
        animator.SetTrigger("AtaqueDisparo");
        estaAtacando = true;

        for (int i = 0; i < 6; i++) // Dispara 6 veces
        {
            yield return StartCoroutine(Disparar());
            yield return new WaitForSeconds(tiempoEntreDisparos);
        }

        estaAtacando = false;
    }

    IEnumerator Disparar()
    {
        // Determinar la dirección en la que el jefe está mirando
        float direccion = transform.localScale.x > 0 ? 1 : -1;

        // Disparar desde el primer punto de disparo
        GameObject proyectil1 = Instantiate(proyectilPrefab, puntoDisparo1.position, Quaternion.identity);
        proyectil1.GetComponent<Proyectil>().SetDireccion(direccion);

        // Esperar un pequeño tiempo antes de disparar desde el segundo punto
        yield return new WaitForSeconds(0.8f);

        // Disparar desde el segundo punto de disparo
        GameObject proyectil2 = Instantiate(proyectilPrefab, puntoDisparo2.position, Quaternion.identity);
        proyectil2.GetComponent<Proyectil>().SetDireccion(direccion);
    }

    public void TomarDaño(int daño)
    {
        if (estaMuerto || enemy == null) return;

        enemy.TakeDamage(daño); // Reducir la vida del jefe

        // Actualizar la barra de vida
        if (healthBar != null)
        {
            healthBar.value = enemy.health; // Actualizar el valor del Slider
            Debug.Log($"Vida del jefe: {enemy.health}, Valor del Slider: {healthBar.value}");
        }

        // Verificar si el jefe ha muerto
        if (enemy.health <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;
        animator.SetTrigger("Muerte");
        rb.linearVelocity = Vector2.zero;

        // Destruir el jefe después de la animación
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);

        // Desactivar la barra de vida
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void Flip(bool faceLeft)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = faceLeft ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !estaMuerto)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Proyectil"))
        {
            int daño = collision.gameObject.GetComponent<Projectile>().damage;
            GetComponent<JefeAI>().TomarDaño(daño);
            Destroy(collision.gameObject); // Destruir el proyectil después de causar daño
        }
    }
}