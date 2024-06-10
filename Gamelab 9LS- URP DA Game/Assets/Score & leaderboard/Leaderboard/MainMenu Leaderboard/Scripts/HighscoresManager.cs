using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoresManager : MonoBehaviour
{
    public static HighscoresManager Instance;
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

    public void AddHighScore(string teamName, string player1Name, string player2Name, int score, int round)
    {
        if (teamName.Length > 15)
        {
            teamName = teamName.Substring(0, 15);
            Debug.Log("Team name truncated to 15 characters.");
        }
        if (player1Name.Length > 10)
        {
            player1Name = player1Name.Substring(0, 10);
            Debug.Log("Player 1 name truncated to 10 characters.");
        }
        if (player2Name.Length > 10)
        {
            player2Name = player2Name.Substring(0, 10);
            Debug.Log("Player 2 name truncated to 10 characters.");
        }

        HighScoreEntry entry = new HighScoreEntry(teamName, player1Name, player2Name, score, round);
        highScoreEntries.Add(entry);
        highScoreEntries.Sort((x, y) => y.score.CompareTo(x.score)); // Sort by score descending
        SaveHighScores();
    }

    public void ClearHighScores()
    {
        PlayerPrefs.DeleteKey("HighScoreTable");
        PlayerPrefs.Save();
        highScoreEntries.Clear();
        ScenesManager.Instance.ResetScene();
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

    public void AddTestScore()
    {
        AddHighScore("testers", "Casper", "Cornee", Mathf.RoundToInt(UnityEngine.Random.Range(2000, 5000)), 5);  // Add a test round number
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
    public int round;

    public HighScoreEntry(string teamName, string player1Name, string player2Name, int score, int round)
    {
        this.teamName = teamName;
        this.player1Name = player1Name;
        this.player2Name = player2Name;
        this.score = score;
        this.round = round;
    }
}

