using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighScoreTable : MonoBehaviour
{
    private Transform entryTemplate;
    private Transform entryContainer;
    [SerializeField]
    private float Templateheight = 20;
    private HighScores highScores;
    private List<Transform> entryTransforms = new List<Transform>();
    public List<HighScoreEntry> highScoreEntries = new List<HighScoreEntry>();

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
        LoadHighScores();
    }

    public void AddHighScore(string teamName, string player1Name, string player2Name, int score)
    {
        HighScoreEntry entry = new HighScoreEntry(teamName, player1Name, player2Name, score);
        highScoreEntries.Add(entry);
        highScoreEntries.Sort((x, y) => y.score.CompareTo(x.score)); // Sort by score descending
        SaveHighScores();
    }

    public void SaveHighScores()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("HighScoreTable", json);
        PlayerPrefs.Save();
    }

    public void LoadHighScores()
    {
        if (PlayerPrefs.HasKey("HighScoreTable"))
        {
            string json = PlayerPrefs.GetString("HighScoreTable");
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    private void CreateHighScoreEntries()
    {
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }
        entryTransforms.Clear();

        foreach (HighScoreEntry entry in highScores.entries)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            entryTransforms.Add(entryTransform);

            int rank = highScores.entries.IndexOf(entry) + 1;
           
            entryTransform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
            entryTransform.Find("TeamName").GetComponent<Text>().text = entry.teamName;
            entryTransform.Find("Player1").GetComponent<Text>().text = entry.player1Name;
            entryTransform.Find("Player2").GetComponent<Text>().text = entry.player2Name;
        }


    }

    public class HighScoreEntry
    {
        public Transform entryTemplate;
        public Transform entryContainer;
        public string teamName;
        public string player1Name;
        public string player2Name;
        public int score;
        public HighScoreEntry(string teamName, string player1Name, string player2Name, int score)
        {
            this.teamName = teamName;
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.score = score;
        }
    }
    public class HighScores
    {
        public List<HighScoreEntry> entries = new List<HighScoreEntry>();
    }
}
