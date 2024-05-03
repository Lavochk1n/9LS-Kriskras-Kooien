using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class EndMenuVariation : MonoBehaviour
{

    public GameObject dropdownVariant, inputVariant; 

    public TMP_Dropdown dropdown1, dropdown2;

    private TestManager manager;

    private void Start()
    {
        manager = TestManager.Instance;


        if (manager.useDropdowns)
        {
            inputVariant.SetActive(false);
            dropdownVariant.SetActive(true);


            dropdown1.AddOptions(new List<string> { manager.name1 , manager.name2 });
            dropdown2.AddOptions(new List<string> { manager.name1 , manager.name2  });
        }
        else
        {
            inputVariant.SetActive(true);
            dropdownVariant.SetActive(false);
        }


    }



}
