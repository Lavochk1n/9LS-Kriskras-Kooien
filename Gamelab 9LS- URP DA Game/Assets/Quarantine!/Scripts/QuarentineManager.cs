
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
        public List<GameObject> Cages = new();

        [Header("Randomiser")]
        private List<AnimalWeight> animalWeights;
        [SerializeField] private int bunnyWeight, crowWeight, parrotWeight, healthyWeight;


        [Header("Game Rules")]
        public float spreadSpeed = 2.1f;
        [SerializeField] int cageQuota = 15;
        [SerializeField] private float completionBonus = 30f;


        [Header("Initialisation")]
        [SerializeField] private List<GameObject> maps = new();
        [SerializeField] private bool randomMap = true;
        [SerializeField] private int mapIndex = 0;

        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        [SerializeField] private GameObject playerPrefab; 




        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this);}  
            else {Instance = this;}



            if (randomMap) { Instantiate(maps[Random.Range(0, maps.Count)]); }
            else { Instantiate(maps[mapIndex]); }

            cagesParent = GameObject.FindGameObjectWithTag("Cage Parent");

            RandomiseCages();
            StartCoroutine(CheckGameProgress()); 
        }

        private void Start()
        {
            var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();

            // Make this less hardcoded 

            var player = Instantiate(
                playerPrefab,
                GameObject.FindGameObjectWithTag("spawn1").transform.position,
                GameObject.FindGameObjectWithTag("spawn1").transform.rotation,
                gameObject.transform);
            player.GetComponent<PlayerBehaviour>().InitializePlayer(playerConfigs[0]);

            var player2 = Instantiate(
                playerPrefab,
                GameObject.FindGameObjectWithTag("spawn2").transform.position,
                GameObject.FindGameObjectWithTag("spawn2").transform.rotation,
                gameObject.transform);
            player2.GetComponent<PlayerBehaviour>().InitializePlayer(playerConfigs[1]);

        }

        private void Update()
        {
            if (!PlayerSpawned())
            {
                return;
            }

            GameManager.Instance.DecreaseTime();

            if (GameOver())
            {
                GameManager.Instance.IncreaseScore(Mathf.RoundToInt(CalculateScore()));
                GameManager.Instance.IncreaseDifficulty();
                GameManager.Instance.AddTime(completionBonus);
                ScenesManager.Instance.NextScene();
            }
        }


        private IEnumerator CheckGameProgress()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (!PlayerSpawned())
                {
                    if (GameOver())
                    {
                        GameManager.Instance.IncreaseScore(Mathf.RoundToInt(CalculateScore()));
                        GameManager.Instance.IncreaseDifficulty();
                        GameManager.Instance.AddTime(completionBonus);
                        ScenesManager.Instance.NextScene();
                    }
                }
            }
        }

        //////////////////////////// GAME RULES TRACKING ////////////////////////////

        /// <returns>true if one of the game-over conditions are met</returns>
        public bool GameOver()
        {

            if (GameManager.Instance.playerBehaviour1.heldAnimal == null ||
                GameManager.Instance.playerBehaviour2.heldAnimal == null)
            {
                return false;
            }



                if (CountInfected() >= cageQuota) return true;

            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.AdjDisease() && cageBehaviour.myAnimal.state == SickState.healthy)
                {
                    return false;
                }
                if (GameManager.Instance.playerBehaviour1.heldAnimal.type != AnimalTypes.Empty ||
                    GameManager.Instance.playerBehaviour2.heldAnimal.type != AnimalTypes.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlayerSpawned()
        {
            if (GameManager.Instance.playerBehaviour1 == null || 
                GameManager.Instance.playerBehaviour2 == null) return false;

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
            
            if (GameManager.Instance.playerBehaviour1.heldAnimal.state == SickState.sick) count++;
            if (GameManager.Instance.playerBehaviour2.heldAnimal.state == SickState.sick) count++;

            

            return count;
        }




        ///////////////// RANDOMISATION////////////////////////////

        private void RandomiseCages()
        {
            animalWeights = new List<AnimalWeight>
            {
                new() {AnimalType = AnimalTypes.Bunny, Weight = bunnyWeight},
                new() {AnimalType = AnimalTypes.crow, Weight = crowWeight},
                new() {AnimalType = AnimalTypes.parrot, Weight = parrotWeight},
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