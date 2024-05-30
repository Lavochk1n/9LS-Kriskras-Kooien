
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        public bool priority = false;
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
        [SerializeField] private int sickAmount = 5;

        [Header("Game Rules")]
        public float spreadSpeed = 2.1f;
        [SerializeField][Range(0, 100.0f)] float cageQuota = 80;
        private int currentDepartures = 0;
        private int totalDepartures;

        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        public bool playerOneUISpawned = false; 
        [SerializeField] private GameObject playerPrefab;

        [Header("GamePause")]
        private bool GamePause, delayRunning;
        [SerializeField] float pauseTimeDelay = 8f;

        [Header("EndOfGameSequence")]
        private bool clearCompleted = false, isClearing = false;
        [SerializeField] private GameObject floatText;
        [SerializeField] private float floatOffset = 2f;
        [SerializeField] private float EndOfGameMalus = .7f; 

        public GameObject player, player2; 

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this);}  
            else {Instance = this;}

            cagesParent = GameObject.FindGameObjectWithTag("Cage Parent");
            totalDepartures = GameManager.Instance.GetTotalDepartures();

            foreach (Transform child in cagesParent.transform)
            {
                Cages.Add(child.gameObject);
            }

            if (TutorialManager.Instance != null)
            {
                
                return;
            }

            RandomiseCages();
            //StartCoroutine(CheckGameProgress()); 
        }

        private void Start()
        {
            InitializePlayers();

            StartCoroutine(DelayedUnpause());
        }

        private void InitializePlayers()
        {
            var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();

            player = Instantiate(
               playerPrefab,
               GameObject.FindGameObjectWithTag("spawn1").transform.position,
               GameObject.FindGameObjectWithTag("spawn1").transform.rotation,
               gameObject.transform);
            player.GetComponent<PlayerBehaviour>().InitializePlayer(playerConfigs[0]);

            player2 = Instantiate(
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

            if (GamePause) { return; }

            if ((clearCompleted))
            {
                isClearing = false;
                clearCompleted = false;
            }


            if (GameOver() && !isClearing)
            {
                ScenesManager.Instance.GetGameOver();
                Debug.Log("GetGameOVer");
                //if (clearCompleted) ScenesManager.Instance.GetGameOver();
                return;

            }

            if (RoomCleared())
            {
                if (!isClearing)
                {
                    AmbulanceManager.Instance.Arrival();
                    StartCoroutine(ClearingRoom());
                    
                    Debug.Log("clear");
                }      
            }
            
        }

    

        //////////////////////////// GAME RULES TRACKING ////////////////////////////

        public bool GamePaused()
        {

            if (GameObject.FindWithTag("spawn1") == null) return true; 


            if (RoomCleared()) return true;     

            if (!PlayerSpawned()) return true; 
            

            return GamePause;
        }

        public void PauseGame()
        {
            if(!delayRunning)
            {
                GamePause = !GamePause; 
            }
        }

        private IEnumerator DelayedUnpause()
        {
            delayRunning = true;

            yield return null;
            GamePause = true;

            yield return new WaitForSeconds(pauseTimeDelay);
            GamePause = false;
            delayRunning = false;
        }

        /// <returns>true if one of the game-over conditions are met</returns>
        public bool RoomCleared()
        {
            if (delayRunning) return false;
  
            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.AdjDisease() > 0 && cageBehaviour.myAnimal.state == SickState.healthy)
                {
                    return false;
                }
                if (cageBehaviour.myAnimal.type == AnimalTypes.Empty )
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

        public bool GameOver()
        {
            if (CountInfected() >= Mathf.RoundToInt(cageQuota * Cages.Count / 100)) return true;
            return false; 
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
            
            return Mathf.RoundToInt(addedScore);
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
            if (GameManager.Instance.playerBehaviour1.heldAnimal == null || GameManager.Instance.playerBehaviour2.heldAnimal == null)
            {
                return 0; 
            }

            if (GameManager.Instance.playerBehaviour1.heldAnimal.state == SickState.sick) count++;
            if (GameManager.Instance.playerBehaviour2.heldAnimal.state == SickState.sick) count++;

            return count;
        }

        private IEnumerator ClearingRoom()
        {
            isClearing = true; 

            float estMin = Cages.Count - cageQuota;
            float estMax = Cages.Count;
            yield return new WaitForSeconds(0.1f);

            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                float performance = 0;
                performance = (100f) - cageBehaviour.myAnimal.sickProgression;
                int AddedScore = Mathf.RoundToInt(performance);

                Vector3 textPos = cage.transform.position;
                textPos.y = cage.transform.position.y + floatOffset;

                GameObject floatTextInstance = Instantiate(floatText, textPos, cage.transform.rotation);
                floatTextInstance.GetComponent<FloatText>().SetScore(AddedScore);
                GameManager.Instance.IncreaseScore(AddedScore);

                cageBehaviour.myAnimal = new Animal()
                {
                    type = AnimalTypes.Empty,
                    state = SickState.healthy,
                    sickProgression = 0,
                };
                cageBehaviour.UpdateCage();

                yield return new WaitForSeconds(0.3f);
            }

            AmbulanceManager.Instance.Departure();
            
            RandomiseCages();
            
            clearCompleted = true;
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

            List<CageBehaviour> potentialCages = new();
            int remainingNeedingSickness = sickAmount; 

            foreach (Transform child in cagesParent.transform)
            {
                CageBehaviour cage = child.GetComponent<CageBehaviour>();
                potentialCages.Add(cage);
                cage.ChangeOccupation(GetWeightedRandomAnimal());
                cage.UpdateCage();
                //cage.ChangeSickstate(GetWeightedRandomState());
            }

            for (int i = 0; i < sickAmount; i++)
            {
                CageBehaviour theChosenOne = potentialCages[UnityEngine.Random.Range(0, potentialCages.Count)];
                theChosenOne.ChangeSickstate(SickState.sick);
                potentialCages.Remove(theChosenOne);
                theChosenOne.UpdateCage();
            }

        }
    
        /// <returns> An animaltype enumstate  based on weight</returns>
        public AnimalTypes GetWeightedRandomAnimal()
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
            return AnimalTypes.Empty;
        }

        public SickState GetWeightedRandomState()
        {
            int totalWeight = 1 + healthyWeight;
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);
            randomWeight -= healthyWeight;
            if (randomWeight < 0)
            {
                return SickState.healthy;
            }
            return SickState.sick;
        }

        //////////////////////// AMBUALNCE
        
        public void AddAmbulanceDepartCounter()
        {
            currentDepartures++;
        }
    }
}