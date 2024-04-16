using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

namespace Quarantine
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        public InventoryUI inventory1, inventory2;



        [Header("Game Rules")]
        public float spreadSpeed = 0.01f;
        [SerializeField] private float gameTime = 150f; 
        [SerializeField] int pointsPerCage = 10, cageQuota = 10;



        [Header("Initialisation")]
        [SerializeField] private List<GameObject> maps = new List<GameObject>();
        [SerializeField] private bool randomMap = true;
        [SerializeField] private int mapIndex=0; 
        public QuarentineManager quarentineManager;

        
        [Header("Other")]
        [SerializeField] private GameObject Winscreen;
        [SerializeField] private TextMeshProUGUI Wintext;
        [SerializeField] private Image timer;
        [SerializeField] private Gradient timercolour; 

        private float playTime = 0f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            if(randomMap) { Instantiate(maps[Random.Range(0, maps.Count)]); }
            else { Instantiate(maps[mapIndex]); }
        }

        private void Update()
        {
            if (quarentineManager == null || inventory1.player == null || inventory2.player == null )
            {
                return;
            }

            if (GameOver())
            {
                if (playTime > gameTime)
                {
                    ScenesManager.Instance.GetGameOver(); 
                }
                else
                {
                    Winscreen.SetActive(true);

                    string text = "Score: " + CalculateScore() + ", playtime:  " + Mathf.RoundToInt(playTime).ToString() + " seconds";

                    Wintext.SetText(text);
                    //ScenesManager.Instance.NextScene();
                }
            }
            else
            {
                playTime += Time.deltaTime;
                timer.fillAmount = 1f - (playTime/gameTime);
                timer.color = timercolour.Evaluate(1f - (playTime / gameTime));
            }
        }


        public bool GameOver()
        {
            if (playTime > gameTime)
            {

                return true;
            }


            if (CountInfected() >= cageQuota) return true;

            foreach (GameObject cage in quarentineManager.Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.AdjDisease() && cageBehaviour.myAnimal.state == sickState.healthy)
                {
                    return false; 
                }
                if (inventory1.player.heldAnimal.type != animalTypes.Empty || inventory2.player.heldAnimal.type != animalTypes.Empty)
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

        private string CalculateScore()
        {
            int score = 0;

            foreach (GameObject cage in quarentineManager.Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.myAnimal.state == sickState.healthy)
                {
                    score += pointsPerCage; 
                }
            }
            return score.ToString(); 
        }

        private int CountInfected()
        {
            int count = 0;

            foreach (GameObject cage in quarentineManager.Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.myAnimal.state == sickState.sick) count++;
            }

            if (inventory1.player.heldAnimal.state == sickState.sick) count++;

            if (inventory2.player.heldAnimal.state == sickState.sick) count++;

            return count;
        }
    }
}