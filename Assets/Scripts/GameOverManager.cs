using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject gameOverCanvas;    
    public Text finalScoreText;

    public GameObject saveScorePanel;
    public InputField nameInputField;

    public AudioClip clickSound;
    public AudioClip cancelSound;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
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

        saveScorePanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnConfirmSave()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);
        string name = nameInputField.text;
        int totalScore = ScoreManager.instance.coinScore + ScoreManager.instance.floorScore;
        LeaderboardManager.instance.AddScore(name, totalScore);
        saveScorePanel.SetActive(false);
    }

    public void OnCancelSave()
    {
        if (cancelSound) audioSource.PlayOneShot(cancelSound);
        saveScorePanel.SetActive(false);
    }

}
