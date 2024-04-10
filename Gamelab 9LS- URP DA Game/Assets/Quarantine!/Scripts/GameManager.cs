using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Quarantine
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        public InventoryUI iventory1, iventory2;



        [Header("Game Rules")]
        [SerializeField] int pointsPerCage = 10;
        public float spreadSpeed = 0.01f;


        [Header("Initialisation")]
        [SerializeField] private List<GameObject> maps = new List<GameObject>();
        [SerializeField] private bool randomMap = true;
        [SerializeField] private int mapIndex=0; 
        public QuarentineManager quarentineManager;

        
        [Header("Other")]
        [SerializeField] private GameObject Winscreen;
        [SerializeField] private TextMeshProUGUI Wintext;

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
            if (quarentineManager == null)
            {
                return;
            }

            if (GameOver())
            {
                Winscreen.SetActive(true);

                string text = "Score: " + CalculateScore() + ", playtime:  " + Mathf.RoundToInt(playTime).ToString() + " seconds";

                Wintext.SetText(text);
            }
            else
            {
                playTime += Time.deltaTime;
            }
        }

        private bool GameOver()
        {
            foreach (GameObject cage in quarentineManager.Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.AdjDisease() && cageBehaviour.myAnimal.state == sickState.healthy)
                {
                    return false;
                }
                
                if (cageBehaviour.myAnimal.type == animalTypes.Empty)
                {
                    return false;
                }
            }
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
    }
}