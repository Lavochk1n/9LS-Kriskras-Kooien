using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveUI : MonoBehaviour
{
    public GameObject uiGlove;
    public GameObject myCanvas;
    public float SpawnOffSet = 100;


    private void Start()
    {
    }

    public void updateUIGlove(int gloves)
    {

        foreach (Transform child in myCanvas.transform)
        {
            Destroy(child.gameObject);
        }
            

        Vector3 spawnPos = new Vector3(50,1000,0);

        for (int i = 0; i < gloves; i++)
        {
            Instantiate(uiGlove, spawnPos, Quaternion.identity, myCanvas.transform);
            spawnPos.y -= SpawnOffSet;
        }
    }

    void HideUIGlove()
    {
        uiGlove.SetActive(false); // Makes the UI element disappear
    }
}
