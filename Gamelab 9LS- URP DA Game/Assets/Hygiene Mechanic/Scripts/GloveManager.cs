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
    [SerializeField] private int maximumgloves = 5;


    private GloveUI gloveUI;

    void Start()
    {
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

    public void UpdateUI()
    {
        GetComponent<PlayerBehaviour>().myUI.GetComponentInChildren<GloveUI>().updateUIGlove(gloves);
    }

    public int GetGloves()
    {
        return gloves;
    }
}

