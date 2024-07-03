
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

    public enum CopyOfSickState
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
        [SerializeField] private int baseSickAmount = 5;

        [Header("Game Rules")]
        public float spreadSpeed = 2.1f;
        [SerializeField][Range(0, 100.0f)] float cageQuota = 80;


        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        public bool playerOneUISpawned = false; 
        [SerializeField] private GameObject playerPrefab;

        [Header("GamePause")]
        private bool GamePause, delayRunning;
        [SerializeField] float pauseTimeDelay = 8f;
        public PauseScreenHandeler pauseScreen;
        [SerializeField] private GameObject countDownCounter;
        [SerializeField] private Sprite start, one, two; 

        [Header("EndOfGameSequence")]
        private bool clearCompleted = false, isClearing = false;
        [SerializeField] private GameObject floatText;
        [SerializeField] private float floatOffset = 2f;
        [SerializeField] private GameObject EndOfGame;
        private bool GameOverlayed = false; 

        private int infectedAmount, infectedQuota;
        private RoundStatisticsDisplay scoreBox; 


        public GameObject player, player2; 

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this);}  
            else {Instance = this;}

            cagesParent = GameObject.FindGameObjectWithTag("Cage Parent");

                

            foreach (Transform child in cagesParent.transform)
            {
                Cages.Add(child.gameObject);
            }

            if (TutorialManager.Instance != null)
            {
                if (TutorialManager.Instance.randomCages) return;
            }

            RandomiseCages();
        }

        private void Start()
        {
            InitializePlayers();

            infectedAmount = CountInfected();
            infectedQuota = Mathf.RoundToInt(cageQuota * Cages.Count / 100);
            if (infectedQuota == 0) { infectedQuota = 10; }

            if (TutorialManager.Instance == null)
            {
                StartCoroutine(DelayedUnpause());
            }
            else
            {
                TutorialManager.Instance.ShowTutorialPopups(0);
            }

            scoreBox = GameObject.FindGameObjectWithTag("ScoreBox").GetComponent<RoundStatisticsDisplay>();
            if (scoreBox != null) scoreBox.RefreshStats(infectedAmount, infectedQuota);

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

                if (TutorialManager.Instance != null)
                {
                    ScenesManager.Instance.GetMainMenu();
                    return;
                }
                if (!GameOverlayed)
                {
                    GameOverlayed = true;
                    Instantiate(EndOfGame);
                }


                //ScenesManager.Instance.GetGameOver();
                //Debug.Log("GetGameOVer");
                return;
            }

            if (RoomCleared())
            {
                if (TutorialManager.Instance != null)
                {
                    if (TutorialManager.Instance.randomCages) return;
                }
                if (!isClearing)
                {
                    StartCoroutine(ClearingRoom());
                    player.GetComponent<GloveManager>().AddGloves();
                    player2.GetComponent<GloveManager>().AddGloves();


                    Debug.Log("clear");
                }      
            }
        }

    

        ///////// GAME RULES TRACKING ////////////

        public bool GamePaused()
        {

            if (GameObject.FindWithTag("spawn1") == null) return true; 

            if (TutorialManager.Instance != null)
            {
                if(TutorialManager.Instance.isShowingPopUp) return true;

                if (!TutorialManager.Instance.randomCages)
                {
                    if (RoomCleared()) return true;
                    if (isClearing) return true;
                }
            }
            else
            {
                if (RoomCleared()) return true;
                if (isClearing) return true;
            }

            if (GameOverlayed) return true;
           

            if (!PlayerSpawned()) return true; 
            

            return GamePause;
        }

        public void PauseGame()
        {
            if(!delayRunning)
            {
                GamePause = !GamePause; 
                pauseScreen.PausePanel.SetActive(GamePause);
            }
        }

        private IEnumerator DelayedUnpause()
        {
            delayRunning = true;
            yield return null;       
            GamePause = true;

            float ExcessWaittime = pauseTimeDelay - 4; 
            if (ExcessWaittime <= 0) { ExcessWaittime = 0.1f; }

            yield return new WaitForSeconds(ExcessWaittime);

            if (TutorialManager.Instance != null)
            {

                TutorialManager.Instance.ShowTutorialPopups(1);
                TutorialManager.Instance.didFirstRound = true;
            }

            while (TutorialManager.Instance != null && TutorialManager.Instance.isShowingPopUp)
            {
                yield return null; 
            }

            GameObject number  = Instantiate(countDownCounter);
            Image numberImg = number.GetComponentInChildren<Image>();
            AudioManager.Instance.PlaySFX(0);
            yield return new WaitForSeconds(1);
            numberImg.sprite = two;
            AudioManager.Instance.PlaySFX(0);

            yield return new WaitForSeconds(1);
            numberImg.sprite = one;
            AudioManager.Instance.PlaySFX(0);

            yield return new WaitForSeconds(1);
            numberImg.sprite = start;
            AudioManager.Instance.PlaySFX(2);

            numberImg.SetNativeSize(); 


            yield return new WaitForSeconds(1);


            Destroy( number );


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
            infectedAmount = CountInfected();
            //infectedQuota = Mathf.RoundToInt(cageQuota * Cages.Count / 100);
            if (scoreBox != null )
            {
                scoreBox.RefreshStats(infectedAmount, infectedQuota);

            }


            if (infectedAmount >= infectedQuota) return true;

            if (TutorialManager.Instance != null)
            {
                if (TutorialManager.Instance.didFirstRound && RoomCleared())
                {
                    return true;
                }
            }

            return false; 
        }

        public bool PlayerSpawned()
        {
            if (GameManager.Instance.playerBehaviour1 == null || 
                GameManager.Instance.playerBehaviour2 == null) return false;

            return true;
        }


        /// <returns>amount of infected tutorialCages</returns>
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

        /// <summary>
        /// routine for clearing the room , granting points and refilling it again. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ClearingRoom()
        {
            baseSickAmount = CountInfected();

            isClearing = true;

            AmbulanceManager.Instance.Arrival();

            yield return new WaitForSeconds(AmbulanceManager.Instance.waitTime);



            foreach (GameObject cage in Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                float performance = 0;
                float malus = cageBehaviour.myAnimal.sickProgression;
                float bonus = 1 +  0.1f * GameManager.Instance.GetDepartures();
                //Debug.Log(bonus);


                if (cageBehaviour.myAnimal.state == SickState.sick) malus = 100f; 

                performance = (100f - malus) * bonus;
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
                cageBehaviour.GetComponent<CageVisual>().ToggleIcon(false);

                yield return new WaitForSeconds(0.1f);
            }

            GameManager.Instance.AddDeparture();
            AmbulanceManager.Instance.Departure();
            RandomiseCages();
            foreach (GameObject cage in Cages)
            {
                CageBehaviour cb = cage.GetComponent<CageBehaviour>();
                cb.ForcedSpreadTick();
            }
            StartCoroutine(DelayedUnpause()); 
            
            clearCompleted = true;
        }

        /// <summary>
        /// Randomizes all tutorialCages with new animals and sickstate. 
        /// </summary>
        private void RandomiseCages()
        {
            animalWeights = new List<AnimalWeight>
            {
                new() {AnimalType = AnimalTypes.Bunny, Weight = bunnyWeight},
                new() {AnimalType = AnimalTypes.crow, Weight = crowWeight},
                new() {AnimalType = AnimalTypes.parrot, Weight = parrotWeight},
            };

            List<CageBehaviour> potentialCages = new();

            foreach (Transform child in cagesParent.transform)
            {
                child.GetComponent<CageVisual>().ToggleIcon(true);

                CageBehaviour cage = child.GetComponent<CageBehaviour>();
                potentialCages.Add(cage);
                cage.ChangeOccupation(GetWeightedRandomAnimal());
                cage.UpdateCage();
                cage.UpdateSpreadSpeed();
            }

            for (int i = 0; i < baseSickAmount; i++)
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

       
    }
}