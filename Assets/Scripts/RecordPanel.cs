using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecordPanel : MonoBehaviour
{
    public GameObject recordEntryPrefab;  
    public Transform recordContainer;     
    public GameObject panel;

    public AudioClip clickSound;
    public AudioClip cancelSound;
    private AudioSource audioSource;

    private void OnEnable()
    {
        RefreshRecords();

        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void RefreshRecords()
    {
        foreach (Transform child in recordContainer)
        {
            Destroy(child.gameObject);
        }

        List<ScoreEntry> records = LeaderboardManager.instance.scores;

        if (records.Count == 0)
        {
            GameObject empty = Instantiate(recordEntryPrefab, recordContainer);
            Text t = empty.GetComponentInChildren<Text>();
            if (t != null)
            {
                t.text = "No records yet.";
            }
            else
            {
                Debug.LogWarning("recordEntryPrefab 中未找到 Text 组件！");
            }
            return;
        }


        float spacing = 80f;
        float topOffset = 150f;
        float totalHeight = records.Count * spacing + topOffset;

        RectTransform contentRT = (RectTransform)recordContainer;
        contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, totalHeight);

        for (int i = 0; i < records.Count; i++)
        {
            ScoreEntry record = records[i];
            GameObject entry = Instantiate(recordEntryPrefab, recordContainer);
            RectTransform rt = entry.GetComponent<RectTransform>();

            rt.offsetMin = new Vector2(0, -spacing);
            rt.offsetMax = new Vector2(0, 0);
            rt.anchoredPosition = new Vector2(0, -topOffset - i * spacing);

            RecordEntryUI ui = entry.GetComponent<RecordEntryUI>();
            ui.SetData(record.playerName, record.score);
        }
    }

    public void ShowPanel()
    {
        if (clickSound) audioSource.PlayOneShot(clickSound);
        panel.SetActive(true);
        RefreshRecords(); 
    }

    public void HidePanel()
    {
        if (cancelSound) audioSource.PlayOneShot(cancelSound);
        panel.SetActive(false);
    }
}
