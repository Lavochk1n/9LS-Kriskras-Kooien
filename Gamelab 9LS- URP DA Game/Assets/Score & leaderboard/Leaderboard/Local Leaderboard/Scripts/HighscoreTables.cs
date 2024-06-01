using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

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
       // HM.AddHighScore("testers", "Casper", "Cornee", 9000);


        entryContainer = GameObject.FindWithTag("Leaderboard").transform;
        //entryTemplate = entryContainer.Find("highscoreEntryTemplate");


        CreateHighScoreEntries();

        
        HM.LoadHighScores();
    }

    private void CreateHighScoreEntries()
    {

        //foreach (Transform child in entryContainer)
        //{
        //    Destroy(child.gameObject);
        //}
        //entryTransforms.Clear();



        for (int i = 0; i < tableSize; i++ )
        {
            HighScoreEntry entry = HM.highScores.entries[i];
            Transform entryTransform = Instantiate(entryTemplate, entryContainer).transform;
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -Templateheight * i);
           


            int rank = HM.highScores.entries.IndexOf(entry) + 1;

            entryTransform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
            entryTransform.Find("TeamName").GetComponent<Text>().text = entry.teamName;
            entryTransform.Find("Player1").GetComponent<Text>().text = entry.player1Name;
            entryTransform.Find("Player2").GetComponent<Text>().text = entry.player2Name;

        }


    }
}