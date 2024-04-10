using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Quarantine
{

    public class CageBehaviour : Interactable
    {

        [SerializeField] private List<CageBehaviour> AdjCages;

        [SerializeField] private LayerMask layer;
        [SerializeField] private float searchDistance =3f;

        private float sickProgression = 0f; 
        private float spreadSpeed;

        [SerializeField] private ProgressionUI progressUI;


        public Animal myAnimal = new Animal();



        private void Start()
        {
            InitializeCages();
            UpdateCage();
            spreadSpeed = GameManager.Instance.spreadSpeed;

            spreadSpeed = spreadSpeed * UnityEngine.Random.Range(.8f, 1.2f);
            if (myAnimal.state == sickState.sick)
            {
                myAnimal.sickProgression = 100f;
            }
            else
            {
                myAnimal.sickProgression = sickProgression;
            }
        
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
            UpdateCage();

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

            progressUI.UpdateVisuals(myAnimal); 
        }
        

        private void CheckSpread()
        {
            if (AdjDisease())
            {
                myAnimal.sickProgression += spreadSpeed;
                UpdateCage();

                if (myAnimal.sickProgression >= 100)
                {
                    myAnimal.state = sickState.sick;
                    
                }
            }
            else if(myAnimal.sickProgression > 0)
            {
                myAnimal.sickProgression -= spreadSpeed;
                UpdateCage();

            }
        }

        public bool AdjDisease()
        {
            foreach(CageBehaviour cage in AdjCages)
            {
                if (cage.IsContagious(myAnimal.type))
                {
                    return true;
                } 
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