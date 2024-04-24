using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quarantine;
using UnityEngine.UI;

public class Test_scriptableOBJ : MonoBehaviour
{

    [SerializeField] private Image _sprite;

    Animal myAnimal  = new Animal(); 


    [SerializeField] private AnimalVisuals myVisuals; 


    // Start is called before the first frame update
    void Start()
    {
        VisualManager animalVisualManager = VisualManager.instance;
        myAnimal.type = AnimalTypes.Bunny;


        myVisuals = animalVisualManager.GetAnimalVisuals(myAnimal.type); 



        _sprite.sprite = myVisuals.iconTypeHealthy;
    }

    // Update is called once per frame
    void Update()
    {

        
        


    }
}
