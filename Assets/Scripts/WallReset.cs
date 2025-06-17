using UnityEngine;

public class WallReset : MonoBehaviour
{
    private Transform respawnPoint;

    private void Start()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Jugador tocó la pared invisible. Regresando al punto de respawn.");
            collision.transform.position = respawnPoint.position;
        }
    }
}
