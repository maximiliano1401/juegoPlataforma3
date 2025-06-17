using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerLives = 3;
    public int playerScore = 0;

    private void Awake()
    {
        // Singleton: asegura que solo haya un GameManager y no se destruya entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLives(int amount)
    {
        playerLives += amount;
    }

    public void UpdateScore(int amount)
    {
        playerScore += amount;
    }
}
