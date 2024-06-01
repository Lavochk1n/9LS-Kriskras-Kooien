using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTables : MonoBehaviour
{
    private Transform entryTemplate;
    private Transform entryContainer;
    [SerializeField]
    private float Templateheight = 20;
    private List<Transform> entryTransforms = new List<Transform>();
    private void Awake()
    {
        entryContainer = GameObject.FindWithTag("Leaderboard").transform;
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < 8; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -Templateheight * i);
            entryTransform.gameObject.SetActive(true);

        }
        //LoadHighScores();
    }

    private void CreateHighScoreEntries()
    {
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }
        entryTransforms.Clear();

        foreach (HighScoreEntry entry in HighscoreManager.Instance.highScores.entries)
        {
            Highscore
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            entryTransforms.Add(entryTransform);

            int rank = highScores.entries.IndexOf(entry) + 1;

            entryTransform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
            entryTransform.Find("TeamName").GetComponent<Text>().text = entry.teamName;
            entryTransform.Find("Player1").GetComponent<Text>().text = entry.player1Name;
            entryTransform.Find("Player2").GetComponent<Text>().text = entry.player2Name;
        }


    }
}