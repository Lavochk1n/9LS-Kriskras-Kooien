using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowcaseRotation : MonoBehaviour
{
    [SerializeField] private float _rotsSpeed =1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, _rotsSpeed, 0);
    }
}
