using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey =
        "9726bd661b4f9557a60a670a5c0e2b33d373b5f18214923dac4f81349a4c599c";
        
    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
         {
             int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
             for (int i = 0; i < loopLength; ++i) {
                 names[i].text = msg[i].Username;
                 scores[i].text = msg[i].Score.ToString();
             }
         }));
    }
    
    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            username.Substring(0, 10);
            GetLeaderboard();
        }));
    }
}
