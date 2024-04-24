using UnityEngine;
using System.Collections.Generic;
using Quarantine;

public class VisualManager : MonoBehaviour
{
    public List<AnimalVisuals> animalVisualsList;

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

    /// <summary>
    /// Find the corresponding visual Scriptable Object 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AnimalVisuals GetAnimalVisuals(AnimalTypes type)
    {
        foreach (AnimalVisuals visuals in animalVisualsList)
        {
            if (visuals.type == type)
                return visuals;
        }

        return null;
    }
}
