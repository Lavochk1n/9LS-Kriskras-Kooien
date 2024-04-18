using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

namespace Quarantine
{
    public class MiniGameManager : MonoBehaviour
    {
        public static MiniGameManager Instance { get; private set; }

        [Header("player Specifics")]
        public bool playerOneSpawned = false;
        public InventoryUI inventory1, inventory2;



        [Header("Game Rules")]
        public float spreadSpeed = 0.01f;
        private float gameTime; 
        [SerializeField] int cageQuota = 10;
        [SerializeField] private float completionBonus = 30f;  



        [Header("Initialisation")]
        [SerializeField] private List<GameObject> maps = new List<GameObject>();
        [SerializeField] private bool randomMap = true;
        [SerializeField] private int mapIndex=0; 
        public QuarentineManager quarentineManager;

        
        



        private float playTime = 0f;

        private void Awake()
        {
            Debug.Log(gameTime);

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

            gameTime = GameManager.instance.GetTimeLeft();
            Debug.Log(gameTime); 
        }

        private void Update()
        {
            if (quarentineManager == null || inventory1.player == null || inventory2.player == null )
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

         
        public bool GameOver()
        {
            
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

        private float CalculateScore()
        {
            float performance = 0;

            foreach (GameObject cage in quarentineManager.Cages)
            {
                CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

                if (cageBehaviour.myAnimal.state == sickState.healthy)
                {
                    performance += 1; 
                }
            }


            float estMin = quarentineManager.Cages.Count - cageQuota;
            float estMax = quarentineManager.Cages.Count;

            float addedScore = (performance - estMin) / ( estMax - estMin) * 100;

            return addedScore; 
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