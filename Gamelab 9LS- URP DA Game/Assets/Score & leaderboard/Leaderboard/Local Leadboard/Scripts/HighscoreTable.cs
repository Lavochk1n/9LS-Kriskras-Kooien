using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighScoreTable : MonoBehaviour
{
    public Transform entryTemplate;
    public Transform entryContainer;
    public Text titleText;
    public Text teamNameText;
    public Text playerNamesText;
    public Text scoreText;
    public Image trophyImage;

    private HighScores highScores;
    private List<Transform> entryTransforms = new List<Transform>();

    private void Awake()
    {
        highScores = new HighScores();
        LoadHighScores();
        CreateHighScoreEntries();
    }

    private void LoadHighScores()
    {
        string json = PlayerPrefs.GetString("HighScoreTable");
        highScores = JsonUtility.FromJson<HighScores>(json);
        if (highScores == null)
        {
            highScores = new HighScores();
        }
        highScores.entries = highScores.entries.OrderByDescending(x => x.score).ToList();
    }

    private void CreateHighScoreEntries()
    {
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }
        entryTransforms.Clear();

        for (int i = 0; i < 8 && i < highScores.entries.Count; i++)
        {
            HighScoreEntry entry = highScores.entries[i];
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            entryTransforms.Add(entryTransform);

            int rank = i + 1;
            string rankString = GetRankString(rank);

            entryTransform.Find("PositionText").GetComponent<Text>().text = rankString;
            entryTransform.Find("TeamNameText").GetComponent<Text>().text = entry.teamName;
            entryTransform.Find("PlayerNamesText").GetComponent<Text>().text = entry.player1Name + " & " + entry.player2Name;
            entryTransform.Find("ScoreText").GetComponent<Text>().text = entry.score.ToString();

            if (rank == 1)
            {
                entryTransform.Find("TrophyImage").GetComponent<Image>().enabled = true;
            }
            else
            {
                entryTransform.Find("TrophyImage").GetComponent<Image>().enabled = false;
            }

            if (rank % 2 == 0)
            {
                entryTransform.Find("Background").GetComponent<Image>().enabled = true;
            }
            else
            {
                entryTransform.Find("Background").GetComponent<Image>().enabled = false;
            }
        }
    }
    public class HighScoreEntry
{
    public int score;
    public string teamName;
    public string player1Name;
    public string player2Name;
}

[System.Serializable]
public class HighScores
{
    public List<HighScoreEntry> entries = new List<HighScoreEntry>();
}



    private string GetRankString(int rank)
    {
        switch (rank)
        {
            case 1:
                return "1st";
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return rank.ToString();
        }
    }

    public void AddHighScoreEntry(int score, string teamName, string player1Name, string player2Name)
    {
        HighScoreEntry newEntry = new HighScoreEntry { score = score, teamName = teamName, player1Name = player1Name, player2Name = player2Name };
        highScores.entries.Add(newEntry);
        highScores.entries = highScores.entries.OrderByDescending(x => x.score).ToList();
        SaveHighScores();
        CreateHighScoreEntries();
    }

    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("HighScoreTable", json);
    }
}