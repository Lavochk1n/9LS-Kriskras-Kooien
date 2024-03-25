using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationButton : MonoBehaviour
{

    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Trash;
    [SerializeField] private float Roatationspeed; 

    public void Rotate()
    {
        if (Target == null)
        {
            Debug.Log("ERROR no target");
            return;
        }

        Target.transform.Rotate(Vector3.up, Roatationspeed * Time.deltaTime);


    }

}
