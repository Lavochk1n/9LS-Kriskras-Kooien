using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quarantine
{

    public class CageBehaviour : Interactable
    {

        [SerializeField] private List<CageBehaviour> AdjCages;

        [SerializeField] private LayerMask layer;
        [SerializeField] private float searchDistance =3f;

        private float sickProgression = -.5f; 
        private float spreadSpeed;

        [SerializeField] private GameObject progressbar, sickIcon; 

        public Animal myAnimal = new Animal();


        private void Start()
        {
            InitializeCages();
            UpdateCage();
            spreadSpeed = QuarentineManager.Instance.spreadSpeed;

            spreadSpeed = spreadSpeed * UnityEngine.Random.Range(.8f, 1.2f); 

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

            AdjCages = new List<CageBehaviour>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, searchDistance);

            foreach (Collider col in colliders)
            {
                CageBehaviour cage = col.GetComponent<CageBehaviour>();
                if (cage != null)
                {
                    AdjCages.Add(cage);
                }
            }

            if (myAnimal.state == sickState.sick)
            {
                sickIcon.SetActive(true);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchDistance);
        }

        public void ChangeOccupation(animalTypes animal)
        {
            myAnimal.type = animal;
        }
        public void ChangeSickstate(sickState sick)
        {
            myAnimal.state = sick;
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

                //else if (myAnimal.sickProgression > 0)
                //{
                //    myAnimal.sickProgression -= spreadSpeed;

                //}
            }
        }

        private bool AdjDisease()
        {
            foreach(CageBehaviour cage in AdjCages)
            {
                if (cage.IsContagious(myAnimal.type))
                {
                    return true;
                } 
            }

            return false;

            //if (AdjCages[0] != null && AdjCages[0].IsContagious(myAnimal.type))
            //{
            //    return true;
            //}

            //if (AdjCages[1] != null && AdjCages[1].IsContagious(myAnimal.type))
            //{
            //    return true;
            //}

            //if (AdjCages[2] != null && AdjCages[2].IsContagious(myAnimal.type))
            //{
            //    return true;
            //}

            //if (AdjCages[3] != null && AdjCages[3].IsContagious(myAnimal.type))
            //{
            //    return true;
            //}
             
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