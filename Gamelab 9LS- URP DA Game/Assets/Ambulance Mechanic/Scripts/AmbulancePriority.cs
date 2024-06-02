using Quarantine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AmbulancePriority : MonoBehaviour
{

    private AmbulanceManager AM;

    public float priorityBonus = 500f; 
    // Start is called before the first frame update
    void Start()
    {
        AM = GetComponent<AmbulanceManager>();
    }
   

    public void RandomPriorityAnimal()
    {
        List<CageBehaviour> potentials = new();

        foreach (GameObject cage in QuarentineManager.Instance.Cages)
        {
            CageBehaviour cb = cage.GetComponent<CageBehaviour>();
            if (cb.myAnimal.state == SickState.sick) potentials.Add(cb);
        }

        int range = potentials.Count;
        CageBehaviour theChosenOne =  potentials[Random.Range(0, range)];
        theChosenOne.myAnimal.priority = true;
        theChosenOne.UpdateCage();

    }

 

    public int CalculateScore()
    {
        int score = 0;
        

        score = Mathf.RoundToInt(priorityBonus * GetComponent<AmbulanceTimer>().CurrentRelativeTime());
        return score; 
    }

    public AnimalTypes RandomPriorityType()
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
