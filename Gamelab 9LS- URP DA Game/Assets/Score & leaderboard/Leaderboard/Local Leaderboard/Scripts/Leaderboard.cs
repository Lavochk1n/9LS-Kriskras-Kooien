using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//public class Leaderboard : MonoBehaviour
//{
//public GameObject highScoreEntryPrefab;
//public Transform highScoreContainer;

//void Start()
//{
//HighScoreTable highScoreTable = FindObjectOfType<HighScoreTable>();
//      List<HighScoreEntry> highScoreEntries = highScoreTable.highScoreEntries;
//
//  for (int i = 0; i < highScoreEntries.Count && i < 50; i++)
//   {
//       HighScoreEntry entry = highScoreEntries[i];
//       GameObject highScoreEntry = Instantiate(highScoreEntryPrefab, highScoreContainer);
//        Text[] texts = highScoreEntry.GetComponentsInChildren<Text>();
//
//         texts[0].text = (i + 1).ToString(); // Rank number
// texts[1].text = entry.teamName;
//          texts[2].text = "Player 1: " + entry.player1Name;
//          texts[3].text = "Player 2: " + entry.player2Name;
//          texts[4].text = entry.score.ToString();
//       }
//  }
//}