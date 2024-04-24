using System.Collections;
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

        [SerializeField] private CageVisual myCageVisual;

        public Animal myAnimal = new Animal();

        


        private void Start()
        {
            InitializeCages();
            spreadSpeed = MiniGameManager.Instance.spreadSpeed;

            spreadSpeed *= Random.Range(.8f, 1.2f) * GameManager.instance.GetDifficultyRatio();
            if (myAnimal.state == SickState.sick)
            {
                myAnimal.sickProgression = 100f;
            }
            else
            {
                myAnimal.sickProgression = sickProgression;
            }

            UpdateCage();


        }

        private void Update()
        {
            if (!MiniGameManager.Instance.PlayerSpawned() || MiniGameManager.Instance.GameOver() ) return;

            CheckSpread();

           
            

        }

        public override void Interact(Interactor interactor)
        {

            PlayerBehaviour playerBehaviour = interactor.GetComponent<PlayerBehaviour>();

            Animal heldAnimal = playerBehaviour.heldAnimal;

            if (heldAnimal.type == AnimalTypes.Empty || myAnimal.type == AnimalTypes.Empty)
            {
                playerBehaviour.heldAnimal = myAnimal;

                myAnimal = heldAnimal;

                UpdateCage();

            }
        }


        public override string GetDescription()
        {

            myCageVisual.IsLookedAt(); 
            return null; 
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

        public void ChangeOccupation(AnimalTypes animal)
        {
            myAnimal.type = animal;
        }
        public void ChangeSickstate(SickState sick)
        {
            myAnimal.state = sick;
        }

        private void UpdateCage()
        {

            myCageVisual.UpdateVisuals(myAnimal); 
        }
        

        private void CheckSpread()
        {
            if (AdjDisease())
            {

                if (myAnimal.sickProgression >= 100)
                {
                    myAnimal.state = SickState.sick;
                    myAnimal.sickProgression = 100;
                }
                else 
                { 
                    myAnimal.sickProgression += spreadSpeed * Time.deltaTime; 
                }

            }
            else if(myAnimal.sickProgression > 0 && myAnimal.state != SickState.sick)
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


        private bool IsContagious(AnimalTypes type)
        {

            if (type == AnimalTypes.Bunny)
            {
                if (myAnimal.type == AnimalTypes.Bunny && myAnimal.state == SickState.sick)
                {
                    return true;
                }
            }

            if (type == AnimalTypes.parrot)
            {
                if (myAnimal.type == AnimalTypes.parrot && myAnimal.state == SickState.sick)
                {
                    return true;
                }
            }

            if (type == AnimalTypes.crow)
            {
                if(myAnimal.type == AnimalTypes.parrot || (myAnimal.type == AnimalTypes.crow && myAnimal.state == SickState.sick))
                {
                    return true;
                }
            }
            return false; 
        }
    }
}