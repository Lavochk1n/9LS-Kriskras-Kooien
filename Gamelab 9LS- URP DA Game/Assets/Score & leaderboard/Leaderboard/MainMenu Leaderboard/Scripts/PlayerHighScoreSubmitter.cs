using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHighScoreSubmitter : MonoBehaviour
{
    private string name1, name2, teamName;
    private int score;
    private GameManager GM; 

    [SerializeField] private TMP_InputField name1tx, name2tx, teamNametx;

    private void Awake()
    {
        GM = GameManager.Instance;
        name1 = GM.playerNames.Name1;
        name2 = GM.playerNames.Name2;
        teamName = GM.playerNames.TeamNAme;
    }


    private void Start()
    {
        name1tx.text = name1;
        name2tx.text = name2;
        teamNametx.text = teamName;
    }


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


    public void saveNames()
    {
        GM.playerNames.SaveNames(name1, name2, teamName);
    }

    public void Submit()
    {
        

        if (string.IsNullOrEmpty(name1) || string.IsNullOrEmpty(name2) || string.IsNullOrEmpty(teamName))  return;  

        Debug.Log(teamName + ": " +  name1 + " & " + name2 );

        GM.playerNames.SaveNames(name1, name2, teamName);

        HighscoreManager.Instance.AddHighScore(teamName, name1, name2, GM.GetScore(), GM.GetDepartures());

        ScenesManager.Instance.GetMainMenu();

    }
}
