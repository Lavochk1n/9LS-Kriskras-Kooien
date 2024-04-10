using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Quarantine
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool playerOneSpawned = false;

        [SerializeField] private List<GameObject> maps = new List<GameObject>();

        public QuarentineManager quarentineManager;

        [SerializeField] int pointsPerCage = 10;

        private float playTime = 0f;

        [SerializeField] private GameObject Winscreen;
        [SerializeField] private TextMeshProUGUI Wintext;

        public float spreadSpeed = 0.01f; 

        
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
            Instantiate(maps[Random.Range(0,maps.Count)]);
        }


        private void Start()
        {
            
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