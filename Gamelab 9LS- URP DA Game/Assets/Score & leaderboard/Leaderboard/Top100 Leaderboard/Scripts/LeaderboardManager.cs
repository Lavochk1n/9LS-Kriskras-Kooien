using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject entryTemplate;
    public Transform entryContainer;
    private HighscoresManager HM;

    private void Start()
    {
        HM = HighscoresManager.Instance;
        HM.LoadHighScores();
        DisplayTopScores();

        
    }

    private void DisplayTopScores()
    {
        List<HighScoreEntry> highScoreEntries = HM.highScoreEntries;

        Debug.Log(highScoreEntries.Count);

        int numberOfEntries = Mathf.Min(highScoreEntries.Count, 100);

        for (int i = 0; i < numberOfEntries; i++)
        {
            HighScoreEntry entry = highScoreEntries[i];
            Transform entryTransform = Instantiate(entryTemplate, entryContainer).transform;
            entryTransform.gameObject.SetActive(true);

            entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            entryTransform.Find("TeamName").GetComponent<TextMeshProUGUI>().text = entry.teamName;
            entryTransform.Find("Player1").GetComponent<TextMeshProUGUI>().text = entry.player1Name;
            entryTransform.Find("Player2").GetComponent<TextMeshProUGUI>().text = entry.player2Name;
            entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
            entryTransform.Find("Round").GetComponent<TextMeshProUGUI>().text = "Round: " + entry.round.ToString();
        }
    }
}
