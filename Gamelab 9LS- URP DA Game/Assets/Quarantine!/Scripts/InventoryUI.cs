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
                case animalTypes.dog:
                   
                    if (player.heldAnimal.state == sickState.sick)
                    {
                        icon.sprite = sickDog;
                    }
                    else
                    {
                        icon.sprite = dog;
                    }

                    break;
                case animalTypes.crow:
                    if (player.heldAnimal.state == sickState.sick)
                    {
                        icon.sprite = sickCrow;
                    }
                    else
                    {
                        icon.sprite = crow;
                    }

                    break;
                case animalTypes.parrot:

                    if (player.heldAnimal.state == sickState.sick)
                    {
                        icon.sprite = sickParrot;
                    }
                    else
                    {
                        icon.sprite = parrot;
                    }

                    break;
                case animalTypes.Empty:

                    icon.sprite = empty;
                    
                    break;
            }
        }
    }
 }

