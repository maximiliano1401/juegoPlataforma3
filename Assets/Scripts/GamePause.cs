using UnityEngine;
using TMPro;

public class GamePause : MonoBehaviour
{
    public TextMeshProUGUI pauseText;
    private bool isPaused = false;

    void Start()
    {
        pauseText.gameObject.SetActive(false); // Ocultar el texto al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;  // Pausar el tiempo del juego
            pauseText.gameObject.SetActive(true);  // Mostrar texto de pausa
        }
        else
        {
            Time.timeScale = 1f;  // Reanudar el tiempo del juego
            pauseText.gameObject.SetActive(false); // Ocultar texto de pausa
        }
    }
}
