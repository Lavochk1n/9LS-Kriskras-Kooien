using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Quarantine;

public class GloveManager : MonoBehaviour
{
    private int gloves;
    private int maximumgloves = 5;


    public GloveUI gloveUI;

    void Start()
    {
        maximumgloves = GameManager.Instance.GetMaxGloves();
        //gloveUI = GetComponent<PlayerBehaviour>().myUI.GetComponentInChildren<GloveUI>();
        gloves = maximumgloves;
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

        gloves = maximumgloves;
        UpdateUI();
    }

    public bool HasGloves()
    {
        if (gloves <= 0)
        {
            gloveUI.spawnFadeOut();
            return false;
            
        }
        return true;
    }

    public void UpdateUI()
    {
        gloveUI.updateUIGlove(gloves);
    }

    public int GetGloves()
    {
        return gloves;
    }
}

