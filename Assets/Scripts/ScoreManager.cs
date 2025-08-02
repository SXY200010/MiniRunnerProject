using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int coinScore = 0;
    public int floorScore = 0;

    public Text scoreText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin()
    {
        coinScore++;
        UpdateUI();
    }

    public void AddFloor()
    {
        floorScore++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            int totalScore = coinScore + floorScore;
            scoreText.text = totalScore.ToString();
        }
        Debug.Log("Score Updated: " + (coinScore + floorScore));
    }
}
