using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTables : MonoBehaviour
{
    private HighscoreManager HM;

    [SerializeField] private GameObject entryTemplate;
    private Transform entryContainer;
    [SerializeField]
    private float Templateheight = 20;
    private List<Transform> entryTransforms = new List<Transform>();

    [SerializeField] private int tableSize = 8;

    private void Start()
    {
        HM = HighscoreManager.Instance;

        HM.LoadHighScores();

        entryContainer = GameObject.FindWithTag("Leaderboard").transform;

        CreateHighScoreEntries();

        HM.LoadHighScores();
    }

    private void CreateHighScoreEntries()
    {
        for (int i = 0; i < tableSize; i++)
        {
            if (i > HM.highScoreEntries.Count - 1 || HM.highScoreEntries == null) return;
            HighScoreEntry entry = HM.highScoreEntries[i];
            Transform entryTransform = Instantiate(entryTemplate, entryContainer).transform;
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -Templateheight * i);

            entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
            entryTransform.Find("TeamName").GetComponent<TextMeshProUGUI>().text = entry.teamName;
            entryTransform.Find("Player1").GetComponent<TextMeshProUGUI>().text = entry.player1Name;
            entryTransform.Find("Player2").GetComponent<TextMeshProUGUI>().text = entry.player2Name;
            entryTransform.Find("Round").GetComponent<TextMeshProUGUI>().text = "Round: " + entry.round.ToString();
        }
    }
}
