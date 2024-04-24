using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Quarantine { 

    public class ProgressionUI : MonoBehaviour
    {
        [SerializeField] private CageBehaviour myCage;

        [SerializeField] private GameObject dogModel, parrotModel, crowModel;

        [SerializeField] private Image panel, progressBar, background;


        [SerializeField] private Sprite crow, sickCrow, parrot, sickParrot, dog, sickDog, empty;

        [SerializeField] private Gradient progressColour, backgroundColour;


        private float progression; 

        private void Start()
        {
            progression = progressBar.fillAmount;
        }

        public void UpdateVisuals(Animal animal)
        {
            UpdateProgressbar(animal);

            UpdateIcon(animal);

        }

        private void UpdateIcon(Animal animal)
        {
            switch (animal.type)
            {
                case AnimalTypes.Bunny:
                    dogModel.SetActive(true);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(false);

                    if (animal.state == SickState.sick)
                    {
                        panel.sprite = sickDog;
                    }
                    else
                    {
                        panel.sprite = dog;
                    }

                    break;
                case AnimalTypes.crow:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(true);

                    if (animal.state == SickState.sick)
                    {
                        panel.sprite = sickCrow;
                    }
                    else
                    {
                        panel.sprite = crow;
                    }

                    break;
                case AnimalTypes.parrot:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(true);
                    crowModel.SetActive(false);

                    if (animal.state == SickState.sick)
                    {
                        panel.sprite = sickParrot;
                    }
                    else
                    {
                        panel.sprite = parrot;
                    }

                    break;
                case AnimalTypes.Empty:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(false);

                    panel.sprite = empty;
                    progressBar.color = Color.white;
                    background.color = Color.white;

                    break;
            }
        }

        private void UpdateProgressbar(Animal animal)
        {
           

            progressBar.fillAmount = animal.sickProgression / 100f;


            if (progression > animal.sickProgression / 100f)
            {
                progressBar.color = progressColour.Evaluate(0f);
                background.color = backgroundColour.Evaluate(0f);
            }
            else
            {
                progressBar.color = progressColour.Evaluate(animal.sickProgression / 100f); 
                background.color =  backgroundColour.Evaluate(animal.sickProgression / 100f);
            }

            

        }


        private void LateUpdate()
        {
            progression = progressBar.fillAmount;
        }
    }

}