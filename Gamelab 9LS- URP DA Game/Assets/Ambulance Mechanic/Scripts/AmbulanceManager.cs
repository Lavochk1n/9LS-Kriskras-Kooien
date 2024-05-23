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


    [SerializeField] private Animator animator;
 [SerializeField] private GameObject floatText; 
    List<Animal> storedAnimals = new List<Animal>();

    [Header("Intervals")]
    private bool HasArrived = false;
    [SerializeField] private float
        awayTime = 30f,
        parkedTime = 8f;
    [SerializeField] private float newGameTime = 40f;
    private float timeLeft;

    [SerializeField] private int ambulanceCapacity = 4; 

    [Header("flickering")]
    [SerializeField] private float flickerThreshold= 5f;
    private bool isFlickering = false;
    private float timerTotal;
    private float flickingInterval = 0.5f;
    [SerializeField] private Renderer greenLight, redLight;


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
        timeLeft = awayTime;
        timerTotal = timeLeft; 

        redLight.material.EnableKeyword("_EMISSION");
        greenLight.material.DisableKeyword("_EMISSION");

        animalPriority = AnimalTypes.Empty;
        priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;
    }
 
    public void DecreaseTime()
    {
        if(QM.GameOver()) { return; }
        if (timeLeft > 0) 
        {
            timeLeft -= Time.deltaTime;
        }
        if (!isFlickering)
        {
            if (timeLeft < timerTotal / 4)
            {
                if (HasArrived)
                {
                    StartCoroutine(FlickerLight(redLight));
                }
                else
                {
                    StartCoroutine(FlickerLight(greenLight));
                }
            }
        }
        
        if (HasArrived)
        {
            if (storedAnimals.Count >= ambulanceCapacity)
            {
                StopAllCoroutines();
                HandleArrival();
                isFlickering = false;
            }
        }
        else
        {
            if (timeLeft < 0)
            {
                StopAllCoroutines();
                HandleArrival();
                isFlickering = false;
            }
        }


        
    }

    public void AddTime(float amount)
    {
        timeLeft += amount;
        
        timerTotal = timeLeft; 
    }

    public void HandleArrival()
    {
        if (!HasArrived)
        {
            Arrival();
            AddTime(parkedTime);
        }
        else
        {
            Departure();
            AddTime(awayTime);
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

    private void Arrival()
    {
        greenLight.material.EnableKeyword("_EMISSION");
        redLight.material.DisableKeyword("_EMISSION");

        animalPriority= RandomPriority();
        priodisplay.sprite = VisualManager.instance.GetAnimalVisuals(animalPriority).iconTypeHealthy;

        animator.GetComponent<Animator>().SetBool("isClosed", false);
        HasArrived = true;
        Debug.Log("arrived");
    }

    private void Departure()
    {
        redLight.material.EnableKeyword("_EMISSION");
        greenLight.material.DisableKeyword("_EMISSION");

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


    private IEnumerator FlickerLight(Renderer light)
    {
        isFlickering = true; 

        while (true)
        {
            light.material.EnableKeyword("_EMISSION");

            if (timeLeft < timerTotal / 8)
            {
                yield return new WaitForSeconds(flickingInterval/2);

            }
            else yield return new WaitForSeconds(flickingInterval);


            light.material.DisableKeyword("_EMISSION");

            if (timeLeft < timerTotal / 8)
            {
                yield return new WaitForSeconds(flickingInterval / 2);

            }
            else yield return new WaitForSeconds(flickingInterval);
        }
    }

    private AnimalTypes RandomPriority()
    {
        int parrotWeight = 0;
        int crowWeight = 0;
        int bunnyWeight = 0; 

        foreach (GameObject cage in QuarentineManager.Instance.Cages)
        {
            CageBehaviour cb = cage.GetComponent<CageBehaviour>();

            switch (cb.myAnimal.type) 
            {
                case AnimalTypes.Bunny:
                    bunnyWeight++; break;
                case AnimalTypes.parrot:
                    parrotWeight++; break;
                case AnimalTypes.crow:
                    crowWeight++; break;
                default:
                    Debug.Log("Error, unknown type");
                    break; 
            }
        }
        int totalWeight = bunnyWeight + crowWeight + parrotWeight;

        if (totalWeight == 0)
        {
            Debug.Log("No animals found.");
            return AnimalTypes.crow;
        }
        int randomWeight = Random.Range(0, totalWeight);

        AnimalTypes selectedAnimalType;
        if (randomWeight < bunnyWeight)
        {
            selectedAnimalType = AnimalTypes.Bunny;
        }
        else if (randomWeight < bunnyWeight + parrotWeight)
        {
            selectedAnimalType = AnimalTypes.parrot;
        }
        else
        {
            selectedAnimalType = AnimalTypes.crow;
        }
        return selectedAnimalType;
    }
}
