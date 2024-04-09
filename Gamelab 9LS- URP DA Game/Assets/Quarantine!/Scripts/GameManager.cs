using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quarantine
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool playerOneSpawned = false;

        [SerializeField] private List<GameObject> maps = new List<GameObject>();



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

    }
}