using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement; 

public class ScenesManager : MonoBehaviour
{

    public static ScenesManager Instance;



    public List<string> miniGames = new List<string>();


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

    }

    public void GetScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void RandomGame()
    {
        SceneManager.LoadScene(miniGames[UnityEngine.Random.Range(0, miniGames.Count)]);
    }


    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void GetMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GetGameOver()
    {
        SceneManager.LoadScene(7);
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

