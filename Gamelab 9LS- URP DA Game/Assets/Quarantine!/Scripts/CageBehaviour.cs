using System.Collections.Generic;
using UnityEngine;


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

            spreadSpeed *= UnityEngine.Random.Range(.8f, 1.2f);
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
            if (GameManager.Instance.GameOver()) return;

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

                if (myAnimal.sickProgression >= 100)
                {
                    myAnimal.state = sickState.sick;
                    myAnimal.sickProgression = 100;
                }
                else 
                { 
                    myAnimal.sickProgression += spreadSpeed * Time.deltaTime; 
                }

            }
            else if(myAnimal.sickProgression > 0 && myAnimal.state != sickState.sick)
            {
                myAnimal.sickProgression -= spreadSpeed * Time.deltaTime;

                if (myAnimal.sickProgression < 0) myAnimal.sickProgression = 0; 

            }
            UpdateCage();

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

            if (type == animalTypes.dog)
            {
                if (myAnimal.type == animalTypes.dog && myAnimal.state == sickState.sick)
                {
                    return true;
                }
            }

            if (type == animalTypes.parrot)
            {
                if (myAnimal.type == animalTypes.parrot && myAnimal.state == sickState.sick)
                {
                    return true;
                }
            }

            if (type == animalTypes.crow)
            {
                if(myAnimal.type == animalTypes.parrot || (myAnimal.type == animalTypes.crow && myAnimal.state == sickState.sick))
                {
                    return true;
                }
            }
            return false; 
        }
    }
}