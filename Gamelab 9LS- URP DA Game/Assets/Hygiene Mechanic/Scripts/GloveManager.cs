using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GloveManager : MonoBehaviour
{
    private int gloves;
    [SerializeField] private int maximumgloves = 5;


    private GloveUI gloveUI;

    void Start()
    {
        gloves = maximumgloves;

        if (GetComponent<PlayerInput>().playerIndex == 0)
        {
            gloveUI = GameObject.FindGameObjectWithTag("gloveUI1").GetComponent<GloveUI>();
        }
        else  gloveUI = GameObject.FindGameObjectWithTag("gloveUI2").GetComponent<GloveUI>();



        //textDisplay = GetComponent<PlayerBehaviour>()
    }

    public void RemoveGlove()
    {
        if (gloves <= 0)
        {
            return;
        }

        gloves--;
        UpdateUI();

        //gloves = gloves - 1;
        //gloves -= 1;

    }

    public void AddGloves()
    {
        if (gloves >= maximumgloves)
        {
            return;
        }

        gloves++;
        UpdateUI();
    }

    public bool HasGloves()
    {
        if (gloves <= 0)
        {
            return false;
        }
        return true;
    }

    private void UpdateUI()
    {
        gloveUI.updateUIGlove(gloves);
    }
}

