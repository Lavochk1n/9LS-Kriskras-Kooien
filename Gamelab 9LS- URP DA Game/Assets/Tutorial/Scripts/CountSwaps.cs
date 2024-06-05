using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountSwaps : MonoBehaviour
{
    private int swaps = 0; 


    // Update is called once per frame
    public void AddSwap()
    {
        swaps++;
    }

    public int Count()
    {
        return swaps;
    }
}
