using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject gameOverCanvas;    
    public Text finalScoreText;          

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        if (finalScoreText != null && ScoreManager.instance != null)
        {
            int totalScore = ScoreManager.instance.coinScore + ScoreManager.instance.floorScore;
            finalScoreText.text =totalScore.ToString();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
