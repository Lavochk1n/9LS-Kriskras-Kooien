using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUI, Player2UI;

    private QuarentineManager QM;

    [SerializeField] private float xBorderoffset = 70f, yBorderoffset = 140f;

    void Start()
    {
        GameObject playerSpecificUI; 

        QM = QuarentineManager.Instance;
        Vector3 spawnPos = new();

        if (!QM.playerOneUISpawned)
        {
            spawnPos.x = xBorderoffset;
            playerSpecificUI = PlayerUI;
            QM.playerOneUISpawned = true;
        }
        else
        {
            spawnPos.x = Screen.width - xBorderoffset;
            playerSpecificUI = Player2UI;
        }

        spawnPos.y = Screen.height- yBorderoffset;
        spawnPos.z = 0; 

        GetComponent<PlayerBehaviour>().myUI = Instantiate(
            playerSpecificUI, spawnPos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform
            );
        GetComponent<GloveManager>().gloveUI = GetComponent<PlayerBehaviour>().myUI.GetComponentInChildren<GloveUI>();
        GetComponent<GloveManager>().UpdateUI();  

    }

   
}
