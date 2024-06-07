using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveCounter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI target, current; 

    private TutorialManager TM; 
        // Start is called before the first frame update
    void Start()
    {
        TM = TutorialManager.Instance;
        target.text = TM.swapTarget.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        current.text = TM.haveSwapped().ToString();
    }
}
