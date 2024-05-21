using Quarantine;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class AmbulanceManager : Interactable
{
    public static AmbulanceManager Instance;


    [Header("Intervals")]
    private bool HasArrived = false;
    [SerializeField] private float
        flaggedInterval = 20f,
        awayTime = 30f,
        parkedTime = 8f;



    [SerializeField] private int ambulanceCapacity = 4; 

    private GameManager GM;
    private QuarentineManager QM;

    [SerializeField] private Animator animator; 

    [SerializeField] private GameObject floatText; 
    List<Animal> storedAnimals = new List<Animal>();

    private float timeLeft;
    [SerializeField] private float newGameTime = 40f;


    [SerializeField] private Renderer greenLight, redLight;

    [SerializeField] private float flickerThreshold= 5f;
    private bool isFlickering = false;
    private float timerTotal;
    private float flickingInterval = 0.5f;


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
        timeLeft = newGameTime;
        redLight.material.EnableKeyword("_EMISSION");
        greenLight.material.DisableKeyword("_EMISSION");
        timerTotal = timeLeft;
    }

    
    public void DecreaseTime()
    {
        if(QM.GameOver()) { return; }
        timeLeft -= Time.deltaTime;
        if (!isFlickering)
        {
            if (timeLeft < timerTotal / 2)
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
        
        if (timeLeft < 0)
        {
            StopAllCoroutines();
            HandleArrival();
            isFlickering = false;
            //timeLeft = newGameTime;

        }
    }

    public void AddTime(float amount)
    {
         
        timeLeft += amount;
        if (timeLeft > newGameTime)
        {
            timeLeft = newGameTime;
        }
        timerTotal = timeLeft; 
    }

    public float GetTotalGameTime()
    {
        return newGameTime;
    }


    public float GetTimeLeft()
    {
        return timeLeft;
    }

    public void HandleArrival()
    {
        if (GM.flaggedMode)
        {
            ArrivalFlagged();
            AddTime(flaggedInterval);
            
        }
        else
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
        float performance = 100f - pb.heldAnimal.sickProgression;
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


        animator.GetComponent<Animator>().SetBool("isClosed", false);
        HasArrived = true;
        Debug.Log("arrived");
        //handle arrival
    }

    private void Departure()
    {
        redLight.material.EnableKeyword("_EMISSION");
        greenLight.material.DisableKeyword("_EMISSION");

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
        textPos.z = transform.position.z -3 ;

        GameObject floatTextInstance = Instantiate(floatText, textPos, transform.rotation, transform);
        floatTextInstance.GetComponent<FloatText>().SetScore(score);
        GameManager.Instance.IncreaseScore(score);
    }

    private IEnumerator CalculateStoreed()
    {

        
        yield return new WaitForSeconds(0.1f);

        foreach (Animal animal in storedAnimals)
        {


            float performance = 100f - animal.sickProgression;

            int AddedScore = Mathf.RoundToInt(performance);

            Vector3 textPos = transform.position;
            textPos.y = transform.position.y + 2;
            textPos.x = transform.position.x + storedAnimals.Count;
            textPos.z = transform.position.z - 1; 


            GameObject floatTextInstance = Instantiate(floatText, textPos, transform.rotation, transform);
            floatTextInstance.GetComponent<FloatText>().SetScore(AddedScore);
            GameManager.Instance.IncreaseScore(AddedScore);
            yield return new WaitForSeconds(0.3f);


        }

        storedAnimals.Clear();

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

    private IEnumerator FlickerLight(Renderer light)
    {
        isFlickering = true; 

        while (true)
        {
            light.material.EnableKeyword("_EMISSION");

            if (timeLeft < timerTotal / 4)
            {
                yield return new WaitForSeconds(flickingInterval/2);

            }
            else yield return new WaitForSeconds(flickingInterval);


            light.material.DisableKeyword("_EMISSION");

            if (timeLeft < timerTotal / 4)
            {
                yield return new WaitForSeconds(flickingInterval / 2);

            }
            else yield return new WaitForSeconds(flickingInterval);
        }
    }
}
