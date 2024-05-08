using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{

    public static TestManager Instance;


    public string name1, name2;

    public bool useDropdowns = true;


    public Image toggle; 


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void SetName1(string name)
    {
        name1 = name;
    }

    public void SetName2(string name)
    {
        name2 = name;
    }

    public void toggleDropdown()
    {
        useDropdowns = !useDropdowns;

        if(useDropdowns)
        {
            toggle.color = Color.white;
        }
        else
        {
            toggle.color = Color.gray;
        }

    }


}
