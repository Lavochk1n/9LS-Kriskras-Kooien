using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextScreenPrompt : MonoBehaviour
{

    [SerializeField] private GameObject hold1, hold2;

    private float holdTimeTarget = 1, holdtime1 = 0, holdtime2 = 0;

    private bool foundPlayers; 

    private PlayerBehaviour p1, p2;
    void Start()
    {
        if (TutorialManager.Instance != null) 
        {
            TutorialManager.Instance.holdIcons = this.gameObject;
            this.gameObject.SetActive(false);

        }


    }



    void Update()
    {
        if (!foundPlayers) 
        {
            p1 = QuarentineManager.Instance.player.GetComponent<PlayerBehaviour>();
            p2 = QuarentineManager.Instance.player2.GetComponent<PlayerBehaviour>();

            if (p1 != null && p2 != null)
            {
                foundPlayers = true;
            }
            else return; 
        }


        if(p1.isHolding)
        {
            holdtime1 += Time.deltaTime; 
        }
        else
        {
            holdtime1 = 0; 
        }

        if (p2.isHolding)
        {
            holdtime2 += Time.deltaTime;
        }
        else
        {
            holdtime2 = 0;
        }

        hold1.GetComponent<Image>().fillAmount = holdtime1;
        hold2.GetComponent<Image>().fillAmount = holdtime2;


        if (holdtime1 > holdTimeTarget && holdtime2 > holdTimeTarget) 
        {

            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.TutorialSequence();
                Debug.Log("next");
            }
            else
            {
                ScenesManager.Instance.GetGameOver(); 
            }

                 
        }

    }
}
