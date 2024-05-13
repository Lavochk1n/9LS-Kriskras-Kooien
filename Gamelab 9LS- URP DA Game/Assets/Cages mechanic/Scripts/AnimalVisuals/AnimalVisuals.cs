using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalVisuals", menuName = "Animal Visuals")]
public class AnimalVisuals : ScriptableObject
{
    public AnimalTypes type; 
    public GameObject model;
    public Sprite iconTypeSick;
    public Sprite iconTypeHealthy;

}
