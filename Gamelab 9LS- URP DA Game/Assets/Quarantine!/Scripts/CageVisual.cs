using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


namespace Quarantine { 

    public class CageVisual : MonoBehaviour
    {
        [SerializeField] private CageBehaviour myCage;

        [SerializeField] private GameObject attachPoint;

        [SerializeField] private Image panel, progressBar, background;


        [SerializeField] private Gradient progressColour, backgroundColour;



        [Header("Light Up")]
        [SerializeField] private GameObject spotLight;
        private bool isLookedAt = false;
        private float lightTimer = 0f;
        [SerializeField] private float resetTime = 0.1f;



        private float progression; 

        private void Start()
        {
            progression = progressBar.fillAmount;
        }


        private void Update()
        {
            spotLight.SetActive(isLookedAt);
            if (lightTimer > 0f)
            {
                lightTimer -= Time.deltaTime;

            }
            else
            {
                isLookedAt = false;
            }
        }

       


        public void UpdateModel(Animal animal)
        {
            if (attachPoint.transform.childCount > 0)
            {


                foreach (Transform child in attachPoint.transform)
                {
                    Destroy(child.gameObject);

                }
            }

            Instantiate(VisualManager.instance.GetAnimalVisuals(animal.type).model, attachPoint.transform);
        }

        public void UpdateIcon(Animal animal)
        {
            AnimalVisuals visuals =  VisualManager.instance.GetAnimalVisuals(animal.type) ;

            if (animal.state == SickState.sick)
            {
                panel.sprite = visuals.iconTypeSick;
            }
            else
            {
                panel.sprite = visuals.iconTypeHealthy;
            }

            if (animal.type == AnimalTypes.Empty)
            {
                progressBar.color = Color.white;
                background.color = Color.white;
            }
        }

        public void UpdateProgressbar(Animal animal)
        {
            if (animal.type == AnimalTypes.Empty)
            {
                progressBar.fillAmount = 0;
                background.color = backgroundColour.Evaluate(0f);
                return; 
            }

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


        public void IsLookedAt()
        {
            isLookedAt = true;
            lightTimer = resetTime;
        }


        private void LateUpdate()
        {
                progression = progressBar.fillAmount;
        }
    }

}