using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Quarantine
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("player Inventory")]
        [SerializeField] private Image background, icon;

        [SerializeField] private Sprite healthy, sickening, sick; 

        public void UpdateInventoryUI(Animal animal)
        {
            AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(animal.type);

            if(animal.state == SickState.sick)
            {
                
                icon.sprite = visuals.iconTypeSick;
                background.sprite = sick;

            }
            else if(animal.sickProgression > 0)
            {
                icon.sprite = visuals.iconTypeSickening;
                background.sprite = sickening;
            }
            else // healthy 
            {   icon.sprite = visuals.iconTypeHealthy;
                background.sprite = healthy;
            }
        }

    }
 }

