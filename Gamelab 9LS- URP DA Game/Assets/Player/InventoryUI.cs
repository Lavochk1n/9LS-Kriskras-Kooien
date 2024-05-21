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


        public void UpdateInventoryUI(Animal animal)
        {
            AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(animal.type);
            if(animal.state == SickState.sick)
            {
                icon.sprite = visuals.iconTypeSick;
            }
            else
            {
                icon.sprite = visuals.iconTypeHealthy;
            }
        }

    }
 }

