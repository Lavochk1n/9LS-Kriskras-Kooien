using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StackThatBulance
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool playerOneSpawned  = false;


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