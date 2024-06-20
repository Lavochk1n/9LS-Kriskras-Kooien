using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveUI : MonoBehaviour
{
    public GameObject uiGlove;
    public GameObject myCanvas;
    public float SpawnOffSet = 100;
    public GameObject fadeOutGlove, fadeOutGlovePlayer; 



    public void updateUIGlove(int gloves)
    {

        foreach (Transform child in myCanvas.transform)
        {
            Destroy(child.gameObject);
        }
            

        Vector3 spawnPos = myCanvas.transform.position;

        for (int i = 0; i < gloves; i++)
        {
            Instantiate(uiGlove, spawnPos, Quaternion.identity, myCanvas.transform);
            spawnPos.y -= SpawnOffSet;
        }
    }

    public void spawnFadeOut(Vector3 pos = default)
    {
        if (pos == default)
        {
            Vector3 spawnPos = myCanvas.transform.position;
            Instantiate(fadeOutGlove, spawnPos, Quaternion.identity, myCanvas.transform);
            spawnPos.y -= SpawnOffSet;
        }
        else
        {
            Vector3 spawnpos = Camera.main.WorldToScreenPoint(pos);
            spawnpos.y += SpawnOffSet;
            Instantiate(fadeOutGlovePlayer, spawnpos, Quaternion.identity, myCanvas.transform);
        }

    }
    void HideUIGlove()
    {
        uiGlove.SetActive(false); // Makes the UI element disappear
    }
}
