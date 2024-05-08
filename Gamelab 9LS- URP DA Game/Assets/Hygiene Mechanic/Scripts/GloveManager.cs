using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GloveManager : MonoBehaviour
{
    private int gloves;
    [SerializeField] private int maximumgloves = 5;

    private TextMeshProUGUI textDisplay; 

    void Start()
    {
        gloves = maximumgloves;
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

    }
}

