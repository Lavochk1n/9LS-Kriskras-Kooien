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


    public float waitTime = 3f;

    public Animator animator;
    [SerializeField] private GameObject floatText; 

    public bool HasArrived = false;



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

    }


    public override void Interact(Interactor interactor)
    {

        
    }

    public override string GetDescription()
    {
        return null;
    }

    public override void Interact_Secondairy(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public void Arrival()
    {
        
        animator.GetComponent<Animator>().SetBool("isClosed", false);
        HasArrived = true;
        Debug.Log("arrived");
    }

    public void Departure()
    {
        

        animator.GetComponent<Animator>().SetBool("isClosed", true);
        HasArrived = false ;
        Debug.Log("departed");


        //QM.AddAmbulanceDepartCounter(); 

        

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
