using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecordEntryUI : MonoBehaviour
{
    public Text nameText;
    public Text scoreText;

    public void SetData(string playerName, int score)
    {
        if (nameText != null)
            nameText.text = playerName;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
