using Quarantine;
using UnityEngine;

namespace StackThatBulance
{
    public class MiniGameManager : MonoBehaviour
    {
        public static MiniGameManager Instance { get; private set; }
        public bool playerOneSpawned = false;

        private bool gameFinished = false;
        [SerializeField] private float completionBonus = 30;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }


        private void Update()
        {
            
            GameManager.instance.DecreaseTime();


            if (GameOver())
            {
                GameManager.instance.IncreaseScore(Mathf.RoundToInt(CalculateScore()));
                GameManager.instance.IncreaseDifficulty();
                GameManager.instance.AddTime(completionBonus);
                ScenesManager.Instance.NextScene();
            }
        }


        private float CalculateScore()
        {
            float performance = 0;

            GameObject[] itemsInBox = GameObject.FindGameObjectsWithTag("Item");
            GameObject[] OtherItemsInBox = GameObject.FindGameObjectsWithTag("OptionalItem");

            foreach (GameObject item in itemsInBox)
            {
                Item itemComponent = item.GetComponent<Item>();
                if (itemComponent != null && itemComponent.isInBox)
                {
                    performance++;
                }
            }
            foreach (GameObject item in OtherItemsInBox)
            {
                Item itemComponent = item.GetComponent<Item>();
                if (itemComponent != null && itemComponent.isInBox)
                {
                    performance++;
                }
            }


            float estMin = itemsInBox.Length/2;
            float estMax = itemsInBox.Length + OtherItemsInBox.Length;

            float addedScore = (performance - estMin) / (estMax - estMin) * 100;

            return addedScore;
        }




        private bool GameOver()
        {
            GameObject[] itemsInBox = GameObject.FindGameObjectsWithTag("Item");
            int totalItems = itemsInBox.Length;
            int itemsCount = 0;

            foreach (GameObject item in itemsInBox)
            {
                Item itemComponent = item.GetComponent<Item>();
                if (itemComponent != null && itemComponent.isInBox)
                {
                    itemsCount++;
                }
            }

            //Debug.Log(itemsCount);
            if (itemsCount == totalItems)
            {
                return true; 
            }
            return false;
        }
    }
}

