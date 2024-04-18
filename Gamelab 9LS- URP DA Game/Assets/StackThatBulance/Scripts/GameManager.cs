using UnityEngine;

namespace StackThatBulance
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool playerOneSpawned = false;

        private bool gameFinished = false;

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

        // Method to finish the game
        public void FinishGame()
        {
            if (!gameFinished)
            {
                // Add your game completion logic here
                Debug.Log("Game finished!");
                gameFinished = true;
            }
        }

    }
}

