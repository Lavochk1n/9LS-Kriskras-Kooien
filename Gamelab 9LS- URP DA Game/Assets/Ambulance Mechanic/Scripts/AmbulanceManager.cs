using Quarantine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    //[Header("priority")]
    //private AnimalTypes animalPriority
    private CageBehaviour animalPriority;

    //[SerializeField] private Image priodisplay;
    //[SerializeField] private float priorityBonus = 1.5f; 

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

        animalPriority = null;
        //animalPriority = AnimalTypes.Empty;
        //priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;
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

        if(pb.heldAnimal.priority)
        {
            pb.heldAnimal.priority = false;
            ShowScoreFloat(Mathf.RoundToInt(Priority.priorityBonus));
        }

        storedAnimals.Add(pb.heldAnimal);
        //float performance = 100f;
        //if (pb.heldAnimal.type == animalPriority) performance *= priorityBonus; 
        //performance -= pb.heldAnimal.sickProgression;

        //int AddedScore = Mathf.RoundToInt(performance);

        //ShowScoreFloat(AddedScore);

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
        Timer.EngageLight(true);
        Priority.RandomPriorityAnimal();
        //animalPriority= Priority.RandomPriorityType();
        //priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;

        //animalPriority = Priority.RandomPriorityAnimal();
        //animalPriority.GetComponent<CageVisual>().UpdateFlag(true); 

        animator.GetComponent<Animator>().SetBool("isClosed", false);
        HasArrived = true;
        Debug.Log("arrived");
    }

    public void Departure()
    {
        Timer.TurnOffFlickering();
        Timer.EngageLight(false);
        //animalPriority = AnimalTypes.Empty;
        //priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;
        //animalPriority = null;
        //animalPriority.GetComponent<CageVisual>().UpdateFlag(false);

        int malus = -1 * Mathf.RoundToInt(Priority.priorityBonus);


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
                    if (UnityEngine.Random.Range(0, 2) == 0)
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



            if (cb.myAnimal.priority)
            {
                Debug.Log("DeductPoints");
                GM.IncreaseScore(malus);
                ShowScoreFloat(malus, cb.transform.position); 
                cb.myAnimal.priority = false;
                cb.UpdateCage();
            }
        }

        Animal p1Animilal = QM.player.GetComponent<PlayerBehaviour>().heldAnimal;
        Animal p2Animilal = QM.player2.GetComponent<PlayerBehaviour>().heldAnimal;


        if (p1Animilal.priority) 
        {
            Debug.Log("DeductPoints");
            GM.IncreaseScore(malus);
            ShowScoreFloat(malus, QM.player.transform.position);
            p1Animilal.priority = false;
        }
        if (p2Animilal.priority)
        {
            Debug.Log("DeductPoints");
            GM.IncreaseScore(malus);
            ShowScoreFloat(malus, QM.player2.transform.position);
            p2Animilal.priority = false;
        }

    }

    private void ShowScoreFloat(int score, Vector3 location = default)
    {
        location.y++;


        if (location == default(Vector3)) 
        {
            location = transform.GetChild(0).transform.position; 
            location.z =- 1.5f; 
        }
        Vector3 textPos = location;

        GameObject floatTextInstance = Instantiate(floatText, textPos, transform.rotation);
        floatTextInstance.GetComponent<FloatText>().SetScore(score);
        GameManager.Instance.IncreaseScore(score);
    }
}
