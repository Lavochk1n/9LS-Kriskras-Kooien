using UnityEngine;
using System.Collections.Generic;
using Quarantine;

public class VisualManager : MonoBehaviour
{
    public List<AnimalVisuals> animalVisualsList;

    // Singleton instance
    public static VisualManager instance;

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

    }

    public AnimalVisuals GetAnimalVisuals(AnimalTypes type)
    {
        // Iterate through the list and find the AnimalVisuals for the specified type
        foreach (AnimalVisuals visuals in animalVisualsList)
        {
            if (visuals.type == type)
                return visuals;
        }

        return null;
    }
}
