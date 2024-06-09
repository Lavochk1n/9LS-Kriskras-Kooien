using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ErrorPreventHandeler : MonoBehaviour
{

    private EventSystem system;
    [SerializeField] private GameObject myObject;
    [SerializeField] private GameObject ObjectToBackOn; 

    private void Awake()
    {
        system = EventSystem.current;
    }


    void OnEnable()
    {
        //ObjectToBackOn = system.currentSelectedGameObject;
        system.SetSelectedGameObject(myObject);
    }

    void OnDisable()
    {
        system.SetSelectedGameObject(ObjectToBackOn);

    }
}
