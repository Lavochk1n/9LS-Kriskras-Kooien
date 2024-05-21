using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUI;

    private QuarentineManager QM; 

    [SerializeField] private float Borderoffset = 70f; 

    void Start()
    {

        QM = QuarentineManager.Instance;
        Vector3 spawnPos = new(); 

        if (!QM.playerOneUISpawned) 
        {
            spawnPos.x = Borderoffset; 
            QM.playerOneUISpawned = true; 
        } 
        else spawnPos.x = Screen.width - Borderoffset;

        spawnPos.y = Screen.height- Borderoffset;
        spawnPos.z = 0; 

        GetComponent<PlayerBehaviour>().myUI = Instantiate(
            PlayerUI, spawnPos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform
            );

        GetComponent<GloveManager>().UpdateUI();  

    }

   
}
