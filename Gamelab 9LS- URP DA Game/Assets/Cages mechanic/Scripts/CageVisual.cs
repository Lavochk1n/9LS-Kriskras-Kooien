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

        [SerializeField] private Sprite backgroundSick, backgroundSickening, backgroundHealthy; 

        [SerializeField] private Color barSickColor, barHealthyColor;

        private bool isLookedAt = false;
        private float lightTimer = 0f;
        [SerializeField] private float resetTime = 0.1f;
        private Material ogMat;
        [SerializeField] private Material lookedAtMAt;

        private float previousProgression;

        
        private void Start()
        {
            previousProgression = progressBar.fillAmount;
            ogMat = GetComponentInChildren<Renderer>().materials[0];
            GetComponentInChildren<Renderer>().materials[0].DisableKeyword("_EMISSION");

        }

        private void Update()
        {


            if(isLookedAt)
            {
                //Debug.Log(isLookedAt);

                GetComponentInChildren<Renderer>().materials[0].EnableKeyword("_EMISSION");

                if (lightTimer > 0f)
                {
                    lightTimer -= Time.deltaTime;
                }
                else //if (mat.IsKeywordEnabled("_EMISSION"))
                {
                    //GetComponentInChildren<Renderer>().materials[0] = ogMat;
                    GetComponentInChildren<Renderer>().materials[0].DisableKeyword("_EMISSION");

                    isLookedAt = false;
                }
            }
            
        }

        public void ToggleIcon(bool state)
        {
            
            panel.transform.parent.gameObject.SetActive(state);
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
                background.sprite = backgroundSickening;
                panel.sprite = visuals.iconTypeHealthy;
            }
        }

        private bool isSickening()
        {
            if (QuarentineManager.Instance.GamePaused()) return false;

            Animal animal  = GetComponent<CageBehaviour>().myAnimal;
            if (animal.sickProgression / 100f > previousProgression)
            {
                return true; 
            }
            else return false;
        }

        public void UpdateProgressbar(Animal animal)
        {
            AnimalVisuals visuals = VisualManager.instance.GetAnimalVisuals(animal.type);

            if (animal.type == AnimalTypes.Empty)
            {
                if (progressBar.IsActive()) progressBar.gameObject.SetActive(false);

                background.sprite = backgroundSickening;
                return; 
            }

            if (animal.state == SickState.sick) 
            {
                if (progressBar.IsActive()) progressBar.gameObject.SetActive(false);
                return;
            }

            if (!progressBar.IsActive())
            {
                progressBar.gameObject.SetActive(true);
            }

            //if (isSickening())
            if (GetComponent<CageBehaviour>().isInfected)
            {   
                panel.sprite = visuals.iconTypeSickening;
                progressBar.color = barSickColor;
                background.sprite = backgroundSickening;
            }
            else
            {
                panel.sprite = visuals.iconTypeHealthy;
                progressBar.color = barHealthyColor;
                background.sprite = backgroundHealthy;
            }
            progressBar.fillAmount = animal.sickProgression / 100f;
        }

        public void IsLookedAt()
        {
            //Debug.Log("register looking");
            isLookedAt = true;
            lightTimer = resetTime;
        }

        private void LateUpdate()
        {
            previousProgression = progressBar.fillAmount;
        }
    }
}