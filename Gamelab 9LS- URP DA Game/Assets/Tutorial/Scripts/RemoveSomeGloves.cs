using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSomeGloves : MonoBehaviour
{
    private bool foundPlayers;

    private GloveManager p1, p2;

    [SerializeField] private int glovesToRemove = 3;

    // Update is called once per frame
    void Update()
    {
        if (foundPlayers)
        {
            return; 
        }
        p1 = QuarentineManager.Instance.player.GetComponent<GloveManager>();
        p2 = QuarentineManager.Instance.player2.GetComponent<GloveManager>();

        if (p1 == null || p2 == null)
        {
            return; 
        }

        foundPlayers = true;

        for (int i = 0; i < glovesToRemove; i++)
        {
            p1.RemoveGlove();
            p2.RemoveGlove();
        }
        
    }
}
