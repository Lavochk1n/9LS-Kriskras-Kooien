
using System.Collections.Generic;
using UnityEngine;

namespace Quarantine
{
    public enum animalTypes
    {
        dog,
        crow,
        parrot,
        Empty
         
    }

    public enum sickState
    {
        healthy,
        sick
    }

    public class Animal
    {
        public animalTypes type;
        public sickState state;
        public float sickProgression;
    }

    public class AnimalWeight
    {
        public animalTypes AnimalType { get; set; }
        public int Weight { get; set; }
    }

    public class QuarentineManager : MonoBehaviour
    {
        public static QuarentineManager Instance { get; private set; }

        [Header("colours")]
        public Material dog;
        public Material native,exotic,empty;

        [Header("cage Set-up")]
        [SerializeField] private GameObject CagePrefab;
        [SerializeField] private int rowCount, rowAmount;
        [SerializeField] private float cageOffset;

        public List<GameObject> Cages = new List<GameObject>();
        private List<AnimalWeight> animalWeights;

        [SerializeField] private int dogWeight, crowWeight, parrotWeight,  healthyWeight;

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
                new AnimalWeight {AnimalType = animalTypes.dog, Weight = dogWeight},
                new AnimalWeight {AnimalType = animalTypes.crow, Weight = crowWeight},
                new AnimalWeight {AnimalType = animalTypes.parrot, Weight = parrotWeight},
            };
        }

        private animalTypes GetWeightedRandomAnimal()
        {
            int totalWeight = 0; 
            foreach (AnimalWeight weight in animalWeights) 
            { 
                totalWeight += weight.Weight;
            }

            int randomWeight = UnityEngine.Random.Range(0, totalWeight);

            for (int i = 0; i < animalWeights.Count; ++i)
            {
                randomWeight -= animalWeights[i].Weight;
                if (randomWeight < 0)
                {
                    return animalWeights[i].AnimalType;
                }
            }
            return animalTypes.Empty;
        }

        private sickState GetWeightedRandomState()
        {
            int totalWeight = 1 + healthyWeight;
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);
            randomWeight -= healthyWeight;
            if (randomWeight < 0)
            {
                return sickState.healthy;
            }
            return sickState.sick;
        }
    }
}