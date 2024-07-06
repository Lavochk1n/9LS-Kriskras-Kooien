using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quarantine
{
    public class CageBehaviour : Interactable
    {
        public List<CageBehaviour> AdjCages { get; private set; }
        public Animal myAnimal = new();
        [SerializeField] private CageVisual myCageVisual;

        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float searchDistance =3f;
        private float spreadSpeed;

        public bool isInfected; 
        public bool markedForRemoval = false;

        public GameObject spPrefab, spreadArrow;

        private void Start()
        {
            gameObject.AddComponent<SpreadParticlesHandeler>();

            InitializeCages();
            UpdateSpreadSpeed();

            if (myAnimal.state == SickState.sick)
            {
                myAnimal.sickProgression = 100f;
            }
            else
            {
                if (TutorialManager.Instance == null)
                {
                    myAnimal.sickProgression = 0f;
                }
            }
            UpdateCage();
            StartCoroutine(UpdateVisuals());
        }

        public void UpdateSpreadSpeed()
        {
            spreadSpeed = QuarentineManager.Instance.spreadSpeed * Random.Range(.98f, 1.02f) * GameManager.Instance.GetDifficultyRatio();
        }

        private void Update()
        {
            if (!QuarentineManager.Instance.PlayerSpawned() || QuarentineManager.Instance.GamePaused() ) return;

            CheckSpread();
        }

        public void ForcedSpreadTick()
        {
            CheckSpread();
            CheckSpread();
            UpdateVisuals();
            UpdateCage();   
        }

        public IEnumerator UpdateVisuals( )
        {
            while (true)
            {
                yield return new WaitForSeconds(.1f);
                myCageVisual.UpdateProgressbar(myAnimal);
            }
        }

        public override void Interact(Interactor interactor)
        {
            PlayerBehaviour playerBehaviour = interactor.GetComponent<PlayerBehaviour>();

            Animal heldAnimal = playerBehaviour.heldAnimal;

            if (heldAnimal.type == AnimalTypes.Empty || myAnimal.type == AnimalTypes.Empty)
            {
                if (markedForRemoval)
                {
                    markedForRemoval = false;
                    playerBehaviour.mostRecentCage.markedForRemoval = true;
                }

                if (heldAnimal.type == AnimalTypes.Empty)
                {
                    if(!playerBehaviour.GetComponent<GloveManager>().HasGloves())
                    {
                        return;
                    }
                    if (TutorialManager.Instance != null) 
                    {
                        if (TutorialManager.Instance.useGloves) { playerBehaviour.GetComponent<GloveManager>().RemoveGlove(); }
                    }
                    else playerBehaviour.GetComponent<GloveManager>().RemoveGlove();
                    GetComponent<CageAudioHandeler>().HandleInteractionSound(myAnimal);
                }
                else
                {
                    GetComponent<CageAudioHandeler>().HandleInteractionSound(heldAnimal);

                    PlayerBehaviour player1 = QuarentineManager.Instance.player.GetComponent<PlayerBehaviour>();
                    PlayerBehaviour player2 = QuarentineManager.Instance.player2.GetComponent<PlayerBehaviour>();

                    if(this != playerBehaviour.mostRecentCage)
                    {
                        playerBehaviour.GetComponent<CountSwaps>().AddSwap();

                    }

                    if (playerBehaviour ==  player1)
                    {
                        if(player2.mostRecentCage == this)
                        {
                            player2.mostRecentCage = player1.mostRecentCage;
                        }
                    }
                    if (playerBehaviour == player2)
                    {
                        if (player1.mostRecentCage == this)
                        {
                            player1.mostRecentCage = player2.mostRecentCage;

                        }
                    }
                }

                playerBehaviour.heldAnimal = myAnimal;
                playerBehaviour.mostRecentCage = this; 
                myAnimal = heldAnimal;

                UpdateCage();
                playerBehaviour.UpdateHeldAnimal(); 
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
                if (col.TryGetComponent<CageBehaviour>(out var cage))
                {
                    if (cage != this)
                    {
                        AdjCages.Add(cage);
                    }
                }
            }
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

        public void UpdateCage()
        {
            myCageVisual.UpdateIcon(myAnimal);
            myCageVisual.UpdateFlag(myAnimal.priority);
        }

        private void CheckSpread()
        {
            int multiplier = 0;
            if (myAnimal.state != SickState.sick)
            {
                 multiplier = AdjDisease();
            }

            if (multiplier > 0)
            {
                isInfected = true;
                if (myAnimal.sickProgression >= 100)
                {
                    GetComponent<CageAudioHandeler>().HandleSickSound(myAnimal.type); 
                    myAnimal.state = SickState.sick;
                    myAnimal.sickProgression = 100;
                    UpdateCage();
                }
                else 
                {
                    myAnimal.sickProgression += spreadSpeed* multiplier * Time.deltaTime ; 
                }
            } 
            else
            {
                isInfected=false;
            }
        }

        /// <returns>multiplier based on the amount of contagious cage </returns>
        public int AdjDisease()
        {
            int multiplier = 0; 
            foreach(CageBehaviour cage in AdjCages)
            {
                bool contaminated = false;  
                if (cage.IsContagious(myAnimal))
                {
                    if (myAnimal.state != SickState.sick)
                    {
                        contaminated = true;
                    }
                    multiplier ++;
                }
            }
            return multiplier;
        }


        /// <returns>Boolean state based on whether the submitted type is suceptible to the occupying animal</returns>
        public bool IsContagious(Animal type)
        {
            if (type.state == SickState.sick) return false;

            if (type.type == AnimalTypes.Bunny)
            {
                if (myAnimal.type == AnimalTypes.Bunny && myAnimal.state == SickState.sick)
                {
                    return true;
                }
            }

            if (type.type == AnimalTypes.parrot)
            {
                if (myAnimal.type == AnimalTypes.parrot && myAnimal.state == SickState.sick)
                {
                    return true;
                }
            }

            if (type.type == AnimalTypes.crow)
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