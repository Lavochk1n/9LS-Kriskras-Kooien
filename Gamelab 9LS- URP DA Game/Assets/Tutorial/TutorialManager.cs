using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public bool lastTutorial = false;

    [Header("Tutorial Cages")]
    public List<CageBehaviour> cages = new();


    public List<AnimalTypes> typePerCage = new();

    public List<SickState> sickStates = new();

    [Range(0,100f)] public List<float> SickProgression = new();



    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        int i = 0;
        foreach (var cage in cages)
        {
            cage.myAnimal.type = typePerCage[i];
            cage.myAnimal.sickProgression = SickProgression[i];
            cage.myAnimal.state = sickStates[i];

            i++;

            cage.UpdateVisuals();
        }
    }

}

