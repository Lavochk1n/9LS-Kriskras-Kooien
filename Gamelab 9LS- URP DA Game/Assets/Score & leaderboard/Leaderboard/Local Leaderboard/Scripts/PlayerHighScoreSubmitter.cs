using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHighScoreSubmitter : MonoBehaviour
{
    private string name1, name2, teamName;
    private int score; 



    public void SetName1(string name)
    {
        name1 = name;
    }

    public void SetName2(string name)
    {
        name2 = name;
    }
    public void SetTeamName(string name)
    {
        teamName = name;
    }


    public void Submit()
    {
        if (name1 == null || name2 == null || teamName == null)  return;  

        Debug.Log(teamName + ": " +  name1 + " & " + name2 );
        
        HighscoreManager.Instance.AddHighScore(teamName, name1, name2, GameManager.Instance.GetScore());

        ScenesManager.Instance.GetMainMenu();

    }
}
