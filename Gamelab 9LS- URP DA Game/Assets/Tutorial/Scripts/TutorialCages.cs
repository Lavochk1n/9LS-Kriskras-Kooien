using Quarantine;
using UnityEngine;

public class TutorialCages : MonoBehaviour
{
    public CageBehaviour cage;
    public AnimalTypes type;
    public SickState sickState;
    [Range(0, 100f)] public float SickProgression;

    private void Awake()
    {
        Animal animal = cage.myAnimal;
        animal.type = type;
        animal.sickProgression = SickProgression;
        animal.state = sickState;

        cage.UpdateVisuals();
    }
}