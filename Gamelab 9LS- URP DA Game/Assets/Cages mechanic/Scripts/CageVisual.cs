using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


namespace Quarantine { 

    public class CageVisual : MonoBehaviour
    {
        [SerializeField] private CageBehaviour myCage;

        [SerializeField] private GameObject attachPoint, flag;

        [SerializeField] private Image panel, progressBar, background;

        [SerializeField] private Sprite backgroundSick, backgroundHealthy; 

        [SerializeField] private Color barSickColor, barHealthyColor;

        [Header("being looked at")]
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
            //spotLight.SetActive(isLookedAt);
            if (lightTimer > 0f)
            {
                lightTimer -= Time.deltaTime;
            }
            else
            {
                isLookedAt = false;
            }
        }

       public void UpdateFlag(bool state)
        {
            flag.SetActive(state);
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




        //public void UpdateIconm(Animal animal)
        //{
        //    AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(animal.type);


        //    if(animal.state  == SickState.sick)
        //    {
        //        background.sprite = backgroundSick;

        //        panel.sprite = visuals.iconTypeSick;
        //    }
        //    else
        //    {
        //        if (progression > animal.sickProgression / 100f)
        //        {
        //            panel.sprite 
        //        }
        //    }
        //}


        public void UpdateIcon(Animal animal)
        {
            AnimalVisuals visuals =  VisualManager.instance.GetAnimalVisuals(animal.type) ;

            if (animal.state == SickState.sick)
            {
                background.sprite = backgroundSick;
                panel.sprite = visuals.iconTypeSick;
            }
            else
            {
                background.sprite = backgroundHealthy;
                panel.sprite = visuals.iconTypeHealthy;
                //progressBar.fillAmount = animal.sickProgression / 100f;
            }

        }

        public void UpdateProgressbar(Animal animal)
        {
            AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(animal.type);

            if (animal.type == AnimalTypes.Empty)
            {
                if (progressBar.IsActive()) progressBar.gameObject.SetActive(false);

                background.sprite = backgroundHealthy;
                return; 
            }

            if (animal.state == SickState.sick) 
            {
                if (progressBar.IsActive()) progressBar.gameObject.SetActive(false);
                //progressBar.fillAmount = animal.sickProgression / 0f;
                return;
            }

            if (!progressBar.IsActive())
            {
                progressBar.gameObject.SetActive(true);
            }

            if (progression >= animal.sickProgression / 100f)
            {
                panel.sprite = visuals.iconTypeHealthy;
                progressBar.color = barHealthyColor;
            }
            else
            {
                panel.sprite = visuals.iconTypeSickening;
                progressBar.color = barSickColor;
            }

            progressBar.fillAmount = animal.sickProgression / 100f;

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