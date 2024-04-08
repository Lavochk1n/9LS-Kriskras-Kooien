using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CageBehaviour : MonoBehaviour
{

    [SerializeField] private CageBehaviour[] AdjCages;

    [SerializeField] private LayerMask layer;
    [SerializeField] private float searchdistance =5f;

    private float sickProgression = -.5f; 
    private float spreadSpeed;

    [SerializeField] private GameObject progressbar, sickIcon; 

    [SerializeField] private animalTypes myAnimal;
    [SerializeField] private sickState myState;

    private void Start()
    {
        InitializeCages();
        UpdateCage();
        spreadSpeed = QuarentineManager.Instance.spreadSpeed;
    }

    private void Update()
    {
        CheckSpread();
    }

    private void InitializeCages()
    {
        UpdateCage();

        AdjCages = new CageBehaviour[4];

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.right, out hit, searchdistance, layer))
        {
            CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
            AdjCages[0] = cage;
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, searchdistance, layer))
        {
            CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
            AdjCages[1] = cage;
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit, searchdistance, layer))
        {
            CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
            AdjCages[2] = cage;
        }
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, searchdistance, layer))
        {
            CageBehaviour cage = hit.transform.GetComponent<CageBehaviour>();
            AdjCages[3] = cage;
        }

        if (myState == sickState.sick)
        {
            sickIcon.SetActive(true);
        }
    }


    public void ChangeOccupation(animalTypes animal)
    {
        myAnimal = animal;
    }
    public void ChangeSickstate(sickState sick)
    {
        myState = sick;
    }


    private void handleSickState()
    {
        switch(myState)
        {
            case sickState.healthy:

                break;
            case sickState.sickening:

                break;
            case sickState.sick:

                break;
            default:
                Debug.Log("ERROR: unknown type");
                break;
        }
    }


    private void UpdateCage()
    {
        Renderer renderer = GetComponent<Renderer>();

        switch(myAnimal)
        {
            case animalTypes.dog:
                renderer.material = QuarentineManager.Instance.dog;

                break;
            case animalTypes.crow:
                renderer.material = QuarentineManager.Instance.native;

                break;
            case animalTypes.parrot:
                renderer.material = QuarentineManager.Instance.exotic;

                break;
            case animalTypes.Empty:
                renderer.material = QuarentineManager.Instance.empty;

                break;
            case animalTypes.closed:
                Destroy(gameObject);
                //renderer.material = QuarentineManager.Instance.closed;

                break;
        }
    }

    private void CheckSpread()
    {
        if(myState == sickState.sick)
        {
            return;
        } 

        if (adjDisease())
        {
            ProgressSickness();
        }
    }

    private void ProgressSickness()
    {
        if (progressbar.activeInHierarchy == false)
        {
            progressbar.SetActive(true);
        }

        sickProgression += spreadSpeed;

        progressbar.GetComponent<Renderer>().material.SetFloat("_progressionRate", sickProgression);

        if(progressbar.GetComponent<Renderer>().material.GetFloat("_progressBorder") < sickProgression)
        {
            myState = sickState.sick;
            progressbar.SetActive(false);
            sickIcon.SetActive(true);
        }
    }

    private bool adjDisease()
    {
        if (AdjCages[0] != null && AdjCages[0].isContagious(myAnimal))
        {
            return true;
        }

        if (AdjCages[1] != null && AdjCages[1].isContagious(myAnimal))
        {
            return true;
        }

        if (AdjCages[2] != null && AdjCages[2].isContagious(myAnimal))
        {
            return true;
        }

        if (AdjCages[3] != null && AdjCages[3].isContagious(myAnimal))
        {
            return true;
        }

        return false; 
    }


    private bool isContagious(animalTypes type)
    {
        if(myAnimal == animalTypes.dog)
        {
            if(type == animalTypes.dog && myState == sickState.sick)
            {
                return true;
            }
        }


        if(myAnimal == animalTypes.parrot)
        {
            if ((type == animalTypes.parrot && myState == sickState.sick) || type == animalTypes.crow)
            {
                return true;
            }
        }


        if (myAnimal == animalTypes.crow)
        {
            if((type == animalTypes.crow || type == animalTypes.parrot) && myState == sickState.sick)
            {
                return true; 
            }
        }

        return false; 
    }


}
