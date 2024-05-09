using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceBehaviour : Interactable
{


    [Header("Intervals")]
    private bool HasArrived = false;
    [SerializeField] private float
        flaggedInterval = 20f,
        fillInterval = 40f,
        parkedTime = 20f;

    [Header("dOOR")]
    public GameObject door;


    private GameManager GM;
    private QuarentineManager QM;


    List<Animal> storedAnimals = new List<Animal>();

    private void Start()
    {
        GM = GameManager.Instance;
        QM = QuarentineManager.Instance;
        GM.AM = this;
    }

    public void HandleArrival()
    {
        if (GM.flaggedMode)
        {
            ArrivalFlagged();
            GM.AddTime(flaggedInterval);
            
        }
        else
        {
            if (!HasArrived)
            {
                Arrival();
                GM.AddTime(parkedTime);
            }
            else
            {
                Departure();
                GM.AddTime(fillInterval);
            }
        }
    }

    public override void Interact(Interactor interactor)
    {

        if (!HasArrived) { return;  }

        PlayerBehaviour pb = interactor.GetComponent<PlayerBehaviour>();

        if (pb.heldAnimal.type == AnimalTypes.Empty)
        {
            return;
        }
        pb.mostRecentCage.markedForRemoval = true;

        storedAnimals.Add(pb.heldAnimal); 

        pb.heldAnimal = new Animal()
        {
            type = AnimalTypes.Empty,
            state = SickState.healthy,
            sickProgression = 0
        };
        pb.UpdateHeldAnimal();


    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact_Secondairy(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }


    private void Arrival()
    {

        door.GetComponent<Animator>().SetBool("isClosed", false);
        HasArrived = true;
        Debug.Log("arrived");
        //handle arrival
    }

    private void Departure()
    {
        door.GetComponent<Animator>().SetBool("isClosed", true);
        HasArrived = false ;
        Debug.Log("departed");


        foreach (Animal animal in  storedAnimals)
        {
            if (animal.state == SickState.healthy)
            {
                GM.IncreaseScore(50);

            }
            else
            {
                GM.IncreaseScore(5);

            }


        }

        foreach (GameObject cage in QuarentineManager.Instance.Cages)
        {
            CageBehaviour cb = cage.GetComponent<CageBehaviour>();

            if (cb.markedForRemoval)
            {
                cb.Interact_Secondairy(null);


                cb.ChangeOccupation(QM.GetWeightedRandomAnimal());
                cb.ChangeSickstate(QM.GetWeightedRandomState());

                if (cb.myAnimal.state == SickState.sick)
                {
                    cb.myAnimal.sickProgression = 100f;
                }
                else
                {
                    cb.myAnimal.sickProgression = 0f;
                }

                cb.UpdateCage();
            }
        }




    }

    public void ArrivalFlagged()
    {
        foreach (GameObject cage in QuarentineManager.Instance.Cages)
        {
            CageBehaviour cb = cage.GetComponent<CageBehaviour>();

            if (cb.markedForRemoval)
            {

                if (cb.myAnimal.state == SickState.healthy)
                {
                    GM.IncreaseScore(50);
                }
                else
                {
                    GM.IncreaseScore(5);
                }
                cb.Interact_Secondairy(null);


                cb.ChangeOccupation(QM.GetWeightedRandomAnimal());
                cb.ChangeSickstate(QM.GetWeightedRandomState());

                if (cb.myAnimal.state == SickState.sick)
                {
                    cb.myAnimal.sickProgression = 100f;
                }
                else
                {
                    cb.myAnimal.sickProgression = 0f;
                }

                cb.UpdateCage();

                GM.playerBehaviour1.flagAmount = GM.playerBehaviour1.maxFlags;
                GM.playerBehaviour2.flagAmount = GM.playerBehaviour2.maxFlags;

            }
        }
    }
}
