using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;


public class FindNameSelectMenu : MonoBehaviour
{

    public PlayerInput pi;

    private string tagName;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pi.playerIndex == 0)
        {
            tagName = "nameMenu0";

        }
        else
        {
            tagName = "nameMenu1";

        }



        var nameMenu = GameObject.FindGameObjectWithTag(tagName);

        if (nameMenu == null)
        {
            Debug.Log("Menu not found");
            return;
        }
        Debug.Log("Menu found");

        pi.uiInputModule = nameMenu.GetComponentInChildren<InputSystemUIInputModule>();

    }

}
