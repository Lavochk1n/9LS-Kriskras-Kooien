using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;


namespace Quarantine { 

    public class ProgressionUI : MonoBehaviour
    {
        [SerializeField] private CageBehaviour myCage;

        [SerializeField] private GameObject dogModel, parrotModel, crowModel;

        [SerializeField] private Image panel, progressBar, background;

        public Dictionary<string, Sprite> animalIcons;

        [SerializeField] private Sprite crow, sickCrow, parrot, sickParrot, dog, sickDog, empty;




        public void UpdateVisuals(Animal animal)
        {

            if (progressBar.fillAmount > 1f - animal.sickProgression / 100f)
            {
                //make it red 
                progressBar.color = Color.red;
                background.color = new Color(1,.5f,.5f,1);

            }
            else if(progressBar.fillAmount <= 0f)
            {
                // everything red
                progressBar.color = Color.red;
                background.color = Color.red;

            }
            else
            {
                progressBar.color = Color.green;
                background.color = Color.white;
            }

            progressBar.fillAmount = 1f - animal.sickProgression/100f;


            switch (animal.type)
            {
                case animalTypes.dog:
                    dogModel.SetActive(true);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(false);

                    if (animal.state == sickState.sick)
                    {
                        panel.sprite = sickDog;
                    }
                    else
                    {
                        panel.sprite = dog;
                    }

                    break;
                case animalTypes.crow:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(true);

                    if (animal.state == sickState.sick)
                    {
                        panel.sprite = sickCrow;
                    }
                    else
                    {
                        panel.sprite = crow;
                    }

                    break;
                case animalTypes.parrot:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(true);
                    crowModel.SetActive(false);

                    if (animal.state == sickState.sick)
                    {
                        panel.sprite = sickParrot;
                    }
                    else
                    {
                        panel.sprite = parrot;
                    }

                    break;
                case animalTypes.Empty:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(false);

                    panel.sprite = empty;
                    progressBar.color = Color.white;
                    background.color = Color.white; 

                    break;
            }


        }
    }
}