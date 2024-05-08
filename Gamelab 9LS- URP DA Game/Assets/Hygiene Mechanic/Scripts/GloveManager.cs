using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveManager : MonoBehaviour
{
    private int gloves;
    [SerializeField] private int maximumgloves = 5;

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
    }

    public bool HasGloves()
    {
        if (gloves <= 0)
        {
            return false;
        }
        return true;
    }
}

