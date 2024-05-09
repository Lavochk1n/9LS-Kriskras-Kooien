using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement; 

public class ScenesManager : MonoBehaviour
{

    public static ScenesManager Instance;

    public List<string> miniGames = new List<string>();

    private bool getTutorial = false; 


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GetScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void GetTutorial()
    {
        getTutorial = true;
        GetPreGame();
    }

    public void RandomGame()
    {
        if (getTutorial) 
        {
            SceneManager.LoadScene("Tutorial0");
            return; 
        }
        SceneManager.LoadScene(miniGames[UnityEngine.Random.Range(0, miniGames.Count)]);
    }

    public void GetMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GetGameOver()
    {
        SceneManager.LoadScene(1);
    }

    public void GetPreGame()
    {
        SceneManager.LoadScene(2);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        UnityEngine.Application.Quit();
    }


}

