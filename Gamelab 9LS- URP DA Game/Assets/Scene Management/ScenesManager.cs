using Quarantine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement; 

public class ScenesManager : MonoBehaviour
{

    public static ScenesManager Instance;

    public List<string> miniGames = new List<string>();

    private GameManager GM;

    
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { 
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        GM = GameManager.Instance;

    }

    public void GetScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void GetTutorial()
    {
        GM.SetTutorial(true);
        SceneManager.LoadScene(3);
    }

    public void RandomGame()
    {
        ResetPlayer();

        AudioManager.Instance.PlayMusic(1); 

        if (GM.IsTutorial()) 
        {
            SceneManager.LoadScene("Tutorial 0");
            //SceneManager.LoadScene("Tutorial 2");

            
            GM.SetTutorial(false);

            return;  
        }
        if (!GM.randomGame)
        {
            SceneManager.LoadScene("GameRoom " + GM.gameRoom.ToString());
            return;

        }
        SceneManager.LoadScene(miniGames[UnityEngine.Random.Range(0, miniGames.Count)]);
    }

    public void GetMainMenu()
    {

        if (PlayerConfigManager.Instance != null) {
            PlayerConfigManager.Instance.ResetConfigs();
            GM.ResetValues();
            Destroy(PlayerConfigManager.Instance.gameObject);
        }


        SceneManager.LoadScene(0);
    }

    public void GetLeaderBoard()
    {
        SceneManager.LoadScene(4); 
    }

    public void GetGameOver()
    {
        AudioManager.Instance.PlayMusic(0);

        ResetPlayer();
        if (TutorialManager.Instance != null) SceneManager.LoadScene(0); 
        else SceneManager.LoadScene(1);
    }

    public void GetPreGame()
    {
        SceneManager.LoadScene(2);
    }

    public void NextScene()
    {
        ResetPlayer();

        if (TutorialManager.Instance != null)
        {
            if(TutorialManager.Instance.lastTutorial) GetMainMenu();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void ResetScene()
    {
        ResetPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        UnityEngine.Application.Quit();
    }

    private void ResetPlayer()
    {
        PlayerBehaviour pb1 = GameManager.Instance.playerBehaviour1;
        PlayerBehaviour pb2 = GameManager.Instance.playerBehaviour2;

        if (pb1 != null)
        {
            Destroy(pb1.gameObject);
            GameManager.Instance.playerBehaviour1 = null;
        }
        if (pb2 != null)
        {
            Destroy(pb2.gameObject);
            GameManager.Instance.playerBehaviour2 = null;
        }
    }
}

