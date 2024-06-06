using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadParticlesHandeler : MonoBehaviour
{

    private GameObject ps_spread;

    private CageBehaviour CB;
    private List<CageBehaviour> adjCages; 

    public List<PSRef> psRefList = new();

    void Start()
    {
        CB = GetComponent<CageBehaviour>();

        ps_spread = CB.spPrefab;
        if (ps_spread == null)
        {
            Debug.LogError("Prefab not found at Assets/Resources/Shaders/VFX_AntCrawl.prefab");
        }

        adjCages = CB.AdjCages;
        if (adjCages != null)
        {
            //InitializeSystems();
        }
    }


    private void InitializeSystems()
    {
        foreach (var cage in adjCages)
        {
            Vector3 pos = cage.transform.position;

            //Vector3 dir =  pos - transform.position;
            //dir = dir.normalized;

            GameObject spawnedObject = Instantiate(ps_spread, transform.position, Quaternion.identity, transform);
            psRefList.Add(new PSRef { cage = cage, ps = spawnedObject });

            spawnedObject.transform.LookAt(pos);

        }
    }


    public void ToggleParticlesForCage(CageBehaviour targetCage, bool state)
    {
        foreach (var psRef in psRefList)
        {
            if (psRef.cage == targetCage)
            {
                var particleSystem = psRef.ps.GetComponentInChildren<ParticleSystem>();
                if (particleSystem != null)
                {
                    if (!state)
                    {
                        particleSystem.Stop();
                    }
                    else
                    {
                        particleSystem.Play();
                    }
                }
                break;
            }
        }
    }



    [System.Serializable]
    public class PSRef
    {
        public CageBehaviour cage;
        public GameObject ps; 
    }

}
