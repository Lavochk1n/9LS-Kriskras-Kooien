using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quarantine
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("player Inventory")]
        public PlayerBehaviour player;
        [SerializeField] private Image background, icon;
        [SerializeField] private Sprite crow, sickCrow, parrot, sickParrot, dog, sickDog, empty;


        void Update()
        {
            if (player == null) { return; }


            switch (player.heldAnimal.type)
            {
                case AnimalTypes.Bunny:
                   
                    if (player.heldAnimal.state == SickState.sick)
                    {
                        icon.sprite = sickDog;
                    }
                    else
                    {
                        icon.sprite = dog;
                    }

                    break;
                case AnimalTypes.crow:
                    if (player.heldAnimal.state == SickState.sick)
                    {
                        icon.sprite = sickCrow;
                    }
                    else
                    {
                        icon.sprite = crow;
                    }

                    break;
                case AnimalTypes.parrot:

                    if (player.heldAnimal.state == SickState.sick)
                    {
                        icon.sprite = sickParrot;
                    }
                    else
                    {
                        icon.sprite = parrot;
                    }

                    break;
                case AnimalTypes.Empty:

                    icon.sprite = empty;
                    
                    break;
            }
        }
    }
 }

