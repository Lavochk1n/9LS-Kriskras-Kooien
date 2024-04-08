using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quarantine
{

    public class CageBehaviour : Interactable
    {

        [SerializeField] private CageBehaviour[] AdjCages;

        [SerializeField] private LayerMask layer;
        [SerializeField] private float searchdistance =5f;

        private float sickProgression = -.5f; 
        private float spreadSpeed;

        [SerializeField] private GameObject progressbar, sickIcon; 

        public Animal myAnimal = new Animal();


        private void Start()
        {
            InitializeCages();
            UpdateCage();
            spreadSpeed = QuarentineManager.Instance.spreadSpeed;
            myAnimal.sickProgression = sickProgression;
        }

        private void Update()
        {
            CheckSpread();
        }

        public override void Interact(Interactor interactor)
        {

            PlayerBehaviour playerBehaviour = interactor.GetComponent<PlayerBehaviour>();

            Animal heldAnimal = playerBehaviour.heldAnimal;

            if (heldAnimal.type == animalTypes.Empty || myAnimal.type == animalTypes.Empty)
            {
                playerBehaviour.heldAnimal = myAnimal;

                myAnimal = heldAnimal;

                UpdateCage();

            }
        }


        public override string GetDescription()
        {
            return "Press 'A' to interact"; 
        }

        private void InitializeCages()
        {
            UpdateCage();

            AdjCages = new CageBehaviour[4];

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.right, out hit, searchdistance, layer))
            {
                CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
                AdjCages[0] = cage;
            }
            if (Physics.Raycast(transform.position, Vector3.back, out hit, searchdistance, layer))
            {
                CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
                AdjCages[1] = cage;
            }
            if (Physics.Raycast(transform.position, Vector3.left, out hit, searchdistance, layer))
            {
                CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
                AdjCages[2] = cage;
            }
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, searchdistance, layer))
            {
                CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
                AdjCages[3] = cage;
            }

            if (myAnimal.state == sickState.sick)
            {
                sickIcon.SetActive(true);
            }
        }

        public void ChangeOccupation(animalTypes animal)
        {
            myAnimal.type = animal;
        }
        public void ChangeSickstate(sickState sick)
        {
            myAnimal.state = sick;
        }

        private void handleSickState()
        {
            switch(myAnimal.state)
            {
                case sickState.healthy:

                    break;

                case sickState.sick:

                    break;
                default:
                    Debug.Log("ERROR: unknown type");
                    break;
            }
        }

        private void UpdateCage()
        {
            Renderer renderer = GetComponent<Renderer>();

            progressbar.SetActive(false);
            sickIcon.SetActive(false);

            if (myAnimal.state == sickState.healthy)
            {
                progressbar.SetActive(false);
                sickIcon.SetActive(false);
            }
            else
            {
                sickIcon.SetActive(true);

            }

            switch (myAnimal.type)
            {
                case animalTypes.dog:
                    renderer.material = QuarentineManager.Instance.dog;

                    break;
                case animalTypes.crow:
                    renderer.material = QuarentineManager.Instance.native;

                    break;
                case animalTypes.parrot:
                    renderer.material = QuarentineManager.Instance.exotic;

                    break;
                case animalTypes.Empty:
                    renderer.material = QuarentineManager.Instance.empty;

                    break;
                case animalTypes.closed:
                    Destroy(gameObject);
                    //renderer.material = QuarentineManager.Instance.closed;

                    break;
            }
        }

        private void CheckSpread()
        {
            if(myAnimal.state == sickState.sick)
            {
                return;
            } 

            
            ProgressSickness();
            
        }

        private void ProgressSickness()
        {
            if (AdjDisease())
            {
                if (progressbar.activeInHierarchy == false)
                {
                    progressbar.SetActive(true);
                }

                myAnimal.sickProgression += spreadSpeed;

                progressbar.GetComponent<Renderer>().material.SetFloat("_progressionRate", myAnimal.sickProgression);

                if (progressbar.GetComponent<Renderer>().material.GetFloat("_progressBorder") < myAnimal.sickProgression)
                {
                    myAnimal.state = sickState.sick;
                    progressbar.SetActive(false);
                    sickIcon.SetActive(true);
                }

                else if (myAnimal.sickProgression > 0)
                {
                    myAnimal.sickProgression -= spreadSpeed;

                }
            }
        }

        private bool AdjDisease()
        {
            if (AdjCages[0] != null && AdjCages[0].IsContagious(myAnimal.type))
            {
                return true;
            }

            if (AdjCages[1] != null && AdjCages[1].IsContagious(myAnimal.type))
            {
                return true;
            }

            if (AdjCages[2] != null && AdjCages[2].IsContagious(myAnimal.type))
            {
                return true;
            }

            if (AdjCages[3] != null && AdjCages[3].IsContagious(myAnimal.type))
            {
                return true;
            }

            return false; 
        }

        private bool IsContagious(animalTypes type)
        {
            if(myAnimal.type == animalTypes.dog)
            {
                if(type == animalTypes.dog && myAnimal.state == sickState.sick)
                {
                    return true;
                }
            }


            if(myAnimal.type == animalTypes.parrot)
            {
                if ((type == animalTypes.parrot && myAnimal.state == sickState.sick) || type == animalTypes.crow)
                {
                    return true;
                }
            }


            if (myAnimal.type == animalTypes.crow)
            {
                if((type == animalTypes.crow || type == animalTypes.parrot) && myAnimal.state == sickState.sick)
                {
                    return true; 
                }
            }

            return false; 
        }


    }


}