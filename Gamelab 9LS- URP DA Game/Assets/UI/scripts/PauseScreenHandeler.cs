using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenHandeler : MonoBehaviour
{
    public GameObject PausePanel;


    private void Start()
    {
        QuarentineManager.Instance.pauseScreen = this; 
    }

    //private void Update()
    //{
    //    if (QuarentineManager.Instance.GamePaused() && !PausePanel.activeInHierarchy)
    //    {
    //        PausePanel.SetActive(true);
    //    }

    //    if (!QuarentineManager.Instance.GamePaused() && PausePanel.activeInHierarchy)
    //    {
    //    }
    //}


    public void Unpause()
    {
        QuarentineManager.Instance.PauseGame();
        PausePanel.SetActive(false);

    }

    public void Surrender()
    {
        ScenesManager.Instance.GetGameOver(); 
    }

    public void Close()
    {
        ScenesManager.Instance.Quit(); 
    }

}
