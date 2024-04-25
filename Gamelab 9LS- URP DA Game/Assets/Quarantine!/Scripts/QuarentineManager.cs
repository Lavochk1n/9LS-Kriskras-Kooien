
using System.Collections;
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
        [SerializeField] private GameObject cagesParent;
        public List<GameObject> Cages = new List<GameObject>();

        [Header("Randomiser")]
        private List<AnimalWeight> animalWeights;
        [SerializeField] private int bunnyWeight, crowWeight, parrotWeight, healthyWeight;


        [Header("Game Rules")]
        public float spreadSpeed = 2.1f;
        private float gameTime;
        [SerializeField] int cageQuota = 15;
        [SerializeField] private float completionBonus = 30f;


        [Header("Initialisation")]
        [SerializeField] private List<GameObject> maps = new List<GameObject>();
        [SerializeField] private bool randomMap = true;
        [SerializeField] private int mapIndex = 0;

        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        public InventoryUI inventory1, inventory2;




        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this);}  
            else {Instance = this;}



            if (randomMap) { Instantiate(maps[Random.Range(0, maps.Count)]); }
            else { Instantiate(maps[mapIndex]); }

            cagesParent = GameObject.FindGameObjectWithTag("Cage Parent");

            gameTime = GameManager.instance.GetTimeLeft();


            RandomiseCages();
            StartCoroutine(CheckGameProgress()); 
        }


        private void Update()
        {
            if ( inventory1.player == null || inventory2.player == null)
            {
                return;
            }

            GameManager.instance.DecreaseTime();

            if (GameOver())
            {
                GameManager.instance.IncreaseScore(Mathf.RoundToInt(CalculateScore()));
                GameManager.instance.IncreaseDifficulty();
                GameManager.instance.AddTime(completionBonus);
                ScenesManager.Instance.NextScene();
            }
        }


        private IEnumerator CheckGameProgress()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (inventory1.player != null && inventory2.player != null)
                {
                    if (GameOver())
                    {
                        GameManager.instance.IncreaseScore(Mathf.RoundToInt(CalculateScore()));
                        GameManager.instance.IncreaseDifficulty();
                        GameManager.instance.AddTime(completionBonus);
                        ScenesManager.Instance.NextScene();
                    }
                }
            }
        }

        //////////////////////////// GAME RULES TRACKING ////////////////////////////

        /// <returns>true if one of the game-over conditions are met</returns>
        public bool GameOver()
        {
            if (CountInfected() >= cageQuota) return true;

            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.AdjDisease() && cageBehaviour.myAnimal.state == SickState.healthy)
                {
                    return false;
                }
                if (inventory1.player.heldAnimal.type != AnimalTypes.Empty || inventory2.player.heldAnimal.type != AnimalTypes.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlayerSpawned()
        {
            if (inventory1.player == null || inventory2.player == null) return false;

            return true;
        }

        /// <returns>score based on the share of healthy cages</returns>
        private float CalculateScore()
        {
            float performance = 0;

            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.myAnimal.state == SickState.healthy)
                {
                    performance += 1;
                }
            }
            float estMin = Cages.Count - cageQuota;
            float estMax = Cages.Count;

            float addedScore = (performance - estMin) / (estMax - estMin) * 100;

            return addedScore;
        }

        /// <returns>amount of infected cages</returns>
        private int CountInfected()
        {
            int count = 0;

            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.myAnimal.state == SickState.sick) count++;
            }

            if (inventory1.player.heldAnimal.state == SickState.sick) count++;
            if (inventory2.player.heldAnimal.state == SickState.sick) count++;

            return count;
        }




        ///////////////// RANDOMISATION////////////////////////////

        private void RandomiseCages()
        {
            animalWeights = new List<AnimalWeight>
            {
                new AnimalWeight {AnimalType = AnimalTypes.Bunny, Weight = bunnyWeight},
                new AnimalWeight {AnimalType = AnimalTypes.crow, Weight = crowWeight},
                new AnimalWeight {AnimalType = AnimalTypes.parrot, Weight = parrotWeight},
            };

            foreach (Transform child in cagesParent.transform)
            {
                Cages.Add(child.gameObject);
                CageBehaviour cage = child.GetComponent<CageBehaviour>();

                cage.ChangeOccupation(GetWeightedRandomAnimal());
                cage.ChangeSickstate(GetWeightedRandomState());
            }
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