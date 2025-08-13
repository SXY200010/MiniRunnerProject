using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    private string savePath => Application.persistentDataPath + "/leaderboard.json";
    public List<ScoreEntry> scores = new List<ScoreEntry>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(string name, int score)
    {
        scores.Add(new ScoreEntry { playerName = name, score = score });
        scores.Sort((a, b) => b.score.CompareTo(a.score)); 
        SaveScores();
    }

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper { list = scores });
        File.WriteAllText(savePath, json);
        Debug.Log("排行榜已保存至: " + savePath);
    }

    public void LoadScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            scores = JsonUtility.FromJson<ScoreListWrapper>(json).list;
        }
    }

    [System.Serializable]
    private class ScoreListWrapper
    {
        public List<ScoreEntry> list;
    }
}
