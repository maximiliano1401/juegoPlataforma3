using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para trabajar con TextMeshPro

public class LevelManager : MonoBehaviour
{
    private int enemiesRemaining;
    public TMP_Text endGameText; // Referencia al texto de fin del juego (TextMeshPro)

    void Start()
    {
        // Contamos los enemigos iniciales
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Asegurarnos de que el texto esté desactivado al inicio
        if (endGameText != null)
        {
            endGameText.gameObject.SetActive(false);
        }
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Verificamos si hay un siguiente nivel
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("¡Felicidades! Has terminado todos los niveles.");
            
            // Mostrar el texto de fin del juego
            if (endGameText != null)
            {
                endGameText.gameObject.SetActive(true);
                endGameText.text = "Juego Completado!";
            }
        }
    }
}
