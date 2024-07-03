using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private GameObject hold1, hold2;

    private float holdTimeTarget = 1, holdtime1 = 0, holdtime2 = 0;
    private bool foundPlayers;

    [SerializeField] private List<GameObject> popUps = new();
    private int index = 0; 
    private GameObject currentPopup;

    private PlayerBehaviour p1, p2;
    void Start()
    {
        ShowNextPopUp();
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


        if (p1.isHolding)
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
            ShowNextPopUp();
            holdtime1 = 0; holdtime2= 0;
        }
    }

    private void ShowNextPopUp()
    {
        if(currentPopup != null) Destroy(currentPopup);
        if (index >= popUps.Count)  
        { 
            gameObject.transform.GetChild(0).gameObject.SetActive(false) ;
            TutorialManager.Instance.isShowingPopUp = false;
            return; 
        }

        Debug.Log("show: " + index);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        TutorialManager.Instance.isShowingPopUp = true;
        currentPopup = Instantiate(popUps[index], gameObject.transform.GetChild(0)); 
        index++;
    }  
}
