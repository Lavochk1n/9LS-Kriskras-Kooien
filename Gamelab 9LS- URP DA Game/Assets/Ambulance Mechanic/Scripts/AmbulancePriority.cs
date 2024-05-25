using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulancePriority : MonoBehaviour
{

    private AmbulanceManager AM; 
    // Start is called before the first frame update
    void Start()
    {
        AM = GetComponent<AmbulanceManager>();
    }


    public AnimalTypes RandomPriority()
    {
        int parrotWeight = 0;
        int crowWeight = 0;
        int bunnyWeight = 0;

        foreach (GameObject cage in QuarentineManager.Instance.Cages)
        {
            CageBehaviour cb = cage.GetComponent<CageBehaviour>();

            switch (cb.myAnimal.type)
            {
                case AnimalTypes.Bunny:
                    bunnyWeight++; break;
                case AnimalTypes.parrot:
                    parrotWeight++; break;
                case AnimalTypes.crow:
                    crowWeight++; break;
                default:
                    Debug.Log("Error, unknown type");
                    break;
            }
        }
        int totalWeight = bunnyWeight + crowWeight + parrotWeight;

        if (totalWeight == 0)
        {
            Debug.Log("No animals found.");
            return AnimalTypes.crow;
        }
        int randomWeight = Random.Range(0, totalWeight);

        AnimalTypes selectedAnimalType;
        if (randomWeight < bunnyWeight)
        {
            selectedAnimalType = AnimalTypes.Bunny;
        }
        else if (randomWeight < bunnyWeight + parrotWeight)
        {
            selectedAnimalType = AnimalTypes.parrot;
        }
        else
        {
            selectedAnimalType = AnimalTypes.crow;
        }
        return selectedAnimalType;
    }
}
