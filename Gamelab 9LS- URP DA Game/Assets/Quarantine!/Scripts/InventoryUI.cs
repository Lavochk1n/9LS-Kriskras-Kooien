using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Quarantine
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("player Inventory")]
        public PlayerBehaviour player;
        [SerializeField] private Image background, icon;


        void Update()
        {
            if (player == null) { return; }

            AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(player.heldAnimal.type); 

            if (player.heldAnimal.state == SickState.sick)
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

