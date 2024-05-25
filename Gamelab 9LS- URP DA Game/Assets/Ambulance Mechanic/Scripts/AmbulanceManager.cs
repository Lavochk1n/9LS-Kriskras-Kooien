using Quarantine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

public class AmbulanceManager : Interactable
{
    public static AmbulanceManager Instance;

    private GameManager GM;
    private QuarentineManager QM;

    private AmbulanceTimer Timer;
    private AmbulancePriority Priority; 


    [SerializeField] private Animator animator;
    [SerializeField] private GameObject floatText; 
    List<Animal> storedAnimals = new List<Animal>();

    public bool HasArrived = false;

    [SerializeField] private int ambulanceCapacity = 4; 

    [Header("priority")]
    private AnimalTypes animalPriority;
    [SerializeField] private Image priodisplay;
    [SerializeField] private float priorityBonus = 1.5f; 

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GM = GameManager.Instance;
        QM = QuarentineManager.Instance;
        Timer = GetComponent<AmbulanceTimer>(); 
        Priority = GetComponent<AmbulancePriority>();




        animalPriority = AnimalTypes.Empty;
        priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;
    }

    public void HandleAmbulance()
    {
        if (QM.GameOver()) { return; }

        if (HasArrived) {
            if (storedAnimals.Count >= ambulanceCapacity || Timer.DecreaseTime())
            {
                Timer.AddTime(); 
                Departure();
            }
        }
        else
        {
            if(Timer.DecreaseTime())
            {
                Timer.AddTime();

                Arrival();
            }
            
        }
    }

    public override void Interact(Interactor interactor)
    {

        if (storedAnimals.Count >= ambulanceCapacity) { }

        if (!HasArrived) { return;  }

        PlayerBehaviour pb = interactor.GetComponent<PlayerBehaviour>();

        if (pb.heldAnimal.type == AnimalTypes.Empty)
        {
            return;
        }
        pb.mostRecentCage.markedForRemoval = true;

        storedAnimals.Add(pb.heldAnimal);
        float performance = 100f;
        if (pb.heldAnimal.type == animalPriority) performance *= priorityBonus; 
        performance -= pb.heldAnimal.sickProgression;

        int AddedScore = Mathf.RoundToInt(performance);

        ShowScoreFloat(AddedScore);



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

    public void Arrival()
    {
        animalPriority= Priority.RandomPriority();
        priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;

        animator.GetComponent<Animator>().SetBool("isClosed", false);
        HasArrived = true;
        Debug.Log("arrived");
    }

    public void Departure()
    {
        animalPriority = AnimalTypes.Empty;
        priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;


        animator.GetComponent<Animator>().SetBool("isClosed", true);
        HasArrived = false ;
        Debug.Log("departed");


        storedAnimals.Clear();

        QM.AddAmbulanceDepartCounter(); 

        foreach (GameObject cage in QuarentineManager.Instance.Cages)
        {
            CageBehaviour cb = cage.GetComponent<CageBehaviour>();

            if (cb.markedForRemoval)
            {
                cb.Interact_Secondairy(null);

                if (TutorialManager.Instance != null)
                {
                    if (Random.Range(0, 1) == 0)
                    {
                        cb.ChangeOccupation(AnimalTypes.crow);
                    }
                    else
                    {
                        cb.ChangeOccupation(AnimalTypes.Bunny);
                    }
                }
                else
                {
                    cb.ChangeOccupation(QM.GetWeightedRandomAnimal());
                }
                cb.ChangeSickstate(QM.GetWeightedRandomState());


                if (cb.myAnimal.state == SickState.sick)
                {
                    cb.myAnimal.sickProgression = 100f;
                }
                else
                {
                    cb.myAnimal.sickProgression = 0f;
                }
                cb.markedForRemoval = false;    
                cb.UpdateCage();
            }
        }
    }

    private void ShowScoreFloat(int score)
    {
        Vector3 textPos = transform.position;
        textPos.z = transform.position.z -1.5f ;

        GameObject floatTextInstance = Instantiate(floatText, textPos, transform.rotation);
        floatTextInstance.GetComponent<FloatText>().SetScore(score);
        GameManager.Instance.IncreaseScore(score);
    }
}
