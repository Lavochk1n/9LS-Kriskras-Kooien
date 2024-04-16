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


        private Color myColor;

        public void SetColour(Color colour)
        {
            myColor = colour;
        }

        // Update is called once per frame
        void Update()
        {
            if (player == null) { return; }

            //background.color = myColor;

            switch (player.heldAnimal.type)
            {
                case animalTypes.dog:
                    //dogModel.SetActive(true);
                    //parrotModel.SetActive(false);
                    //crowModel.SetActive(false);

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
                    //dogModel.SetActive(false);
                    //parrotModel.SetActive(false);
                    //crowModel.SetActive(true);

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
                    //dogModel.SetActive(false);
                    //parrotModel.SetActive(true);
                    //crowModel.SetActive(false);

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
                    //dogModel.SetActive(false);
                    //parrotModel.SetActive(false);
                    //crowModel.SetActive(false);

                    icon.sprite = empty;
                    
                    break;
            }


        }


    }


    }

