using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public bool lastTutorial = false;
    public bool randomCages = false;
    public bool useGloves = true;


    public int swapTarget = 3;

    public GameObject holdIcons;

    //[Header("Tutorial Cages")]
    //public List<TutorialCages> tutorialCages = new();


    private void Start()
    {
        QuarentineManager.Instance.PauseGame();
    }

    private void Update()
    {

        if ( randomCages)
        {
            if (haveSwapped() >= swapTarget)
            {
                holdIcons.SetActive(true);
            }
        }
        
    }

    public void TutorialSequence()
    {
        ScenesManager.Instance.NextScene();

    }



    public int haveSwapped()
    {
        var QM = QuarentineManager.Instance;
        int swaps  = QM.player.GetComponent<CountSwaps>().Count() +  QM.player2.GetComponent<CountSwaps>().Count();
        return swaps;
    }


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        //foreach (var tut in tutorialCages)
        //{
        //    Animal animal = tut.cage.myAnimal;
        //    animal.type = tut.type;
        //    animal.sickProgression = tut.SickProgression; 
        //    animal.state = tut.sickState;

        //    tut.cage.UpdateVisuals();
        //}
    }

    

}



