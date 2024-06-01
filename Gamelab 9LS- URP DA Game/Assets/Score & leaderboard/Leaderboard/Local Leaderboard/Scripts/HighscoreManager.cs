using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance;
    public HighScores highScores;
    public List<HighScoreEntry> highScoreEntries = new List<HighScoreEntry>();
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        highScores.entries = highScoreEntries;
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
        highScoreEntries = highScores.entries;
    }
    
}

[System.Serializable]
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

[System.Serializable]
public class HighScores
{
    public List<HighScoreEntry> entries = new List<HighScoreEntry>();
}