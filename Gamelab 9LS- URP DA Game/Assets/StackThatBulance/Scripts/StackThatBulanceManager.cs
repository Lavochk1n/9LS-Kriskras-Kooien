using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackThatBulance
{

    public class StackThatBulanceManager : MonoBehaviour
    {
        public static StackThatBulanceManager Instance { get; private set; }
        public bool playerOneSpawned; 

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

    }
}