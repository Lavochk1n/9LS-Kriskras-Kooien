using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class FindNameSelectMenu : MonoBehaviour
{

    public PlayerInput pi;

    private void Awake()
    {

        string tagName = "nameMenu" + pi.playerIndex.ToString();

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
