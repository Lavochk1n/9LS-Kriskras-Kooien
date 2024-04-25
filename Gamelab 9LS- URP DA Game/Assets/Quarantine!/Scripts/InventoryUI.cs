using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quarantine
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("player Inventory")]
        //public PlayerBehaviour player;
        [SerializeField] private Image background, icon;

        [SerializeField] private bool player1Inventory;

        private PlayerBehaviour playerBehaviour;


       

        void Update()
        {
            if (playerBehaviour == null) 
            {
                if (player1Inventory)
                {
                    playerBehaviour = GameManager.Instance.playerBehaviour1;
                }
                else
                {
                    playerBehaviour = GameManager.Instance.playerBehaviour2;

                }; 
                return;
            }

            AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(playerBehaviour.heldAnimal.type); 

            if (playerBehaviour.heldAnimal.state == SickState.sick)
            {
                icon.sprite =  visuals.iconTypeSick;
            }
            else
            {
                icon.sprite = visuals.iconTypeHealthy;
            }       
        }
    }
 }

