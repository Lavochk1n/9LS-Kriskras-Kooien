
using System.Collections.Generic;
using UnityEngine;

namespace Quarantine
{
    public enum AnimalTypes
    {
        Bunny,
        crow,
        parrot,
        Empty
         
    }

    public enum SickState
    {
        healthy,
        sick
    }

    public class Animal
    {
        public AnimalTypes type;
        public SickState state;
        public float sickProgression;
    }


    public class AnimalWeight
    {
        public AnimalTypes AnimalType { get; set; }
        public int Weight { get; set; }
    }

    public class QuarentineManager : MonoBehaviour
    {
        public static QuarentineManager Instance { get; private set; }

        [Header("cage Set-up")]
        [SerializeField] private GameObject CagePrefab;
        public List<GameObject> Cages = new List<GameObject>();

        [Header("Randomiser")]
        private List<AnimalWeight> animalWeights;
        [SerializeField] private int bunnyWeight, crowWeight, parrotWeight, healthyWeight;






        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this);}  
            else {Instance = this;}

            GetAnimalWeights();

            foreach (Transform child  in transform) 
            {
                Cages.Add(child.gameObject);
                CageBehaviour cage = child.GetComponent<CageBehaviour>();

                cage.ChangeOccupation(GetWeightedRandomAnimal());
                cage.ChangeSickstate(GetWeightedRandomState());
            }
            MiniGameManager.Instance.quarentineManager = this; 
        }

        private void GetAnimalWeights()
        {
            animalWeights = new List<AnimalWeight>
            {
                new AnimalWeight {AnimalType = AnimalTypes.Bunny, Weight = bunnyWeight},
                new AnimalWeight {AnimalType = AnimalTypes.crow, Weight = crowWeight},
                new AnimalWeight {AnimalType = AnimalTypes.parrot, Weight = parrotWeight},
            };
        }

      
        /// <returns> An animaltype enumstate  based on weight</returns>
        private AnimalTypes GetWeightedRandomAnimal()
        {
            int totalWeight = 0; 
            foreach (AnimalWeight weight in animalWeights) 
            { 
                totalWeight += weight.Weight;
            }

            int randomWeight = Random.Range(0, totalWeight);

            for (int i = 0; i < animalWeights.Count; ++i)
            {
                randomWeight -= animalWeights[i].Weight;
                if (randomWeight < 0)
                {
                    return animalWeights[i].AnimalType;
                }
            }
            return AnimalTypes.Empty;
        }

        private SickState GetWeightedRandomState()
        {
            int totalWeight = 1 + healthyWeight;
            int randomWeight = Random.Range(0, totalWeight);
            randomWeight -= healthyWeight;
            if (randomWeight < 0)
            {
                return SickState.healthy;
            }
            return SickState.sick;
        }
    }
}