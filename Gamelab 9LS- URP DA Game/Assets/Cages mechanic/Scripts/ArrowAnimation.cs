using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnimation : MonoBehaviour
{
    private Image arrow;
    [SerializeField] private float bobDistance = 2f, bobSpeed = 0.1f;

    private float timer;

    private Vector3 ogPos; 

    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        arrow = GetComponent<Image>();

        timer = bobDistance;
        StartCoroutine(Bob());

    }


    private IEnumerator Bob()
    {
        while (true)
        {
            yield return StartCoroutine(Move(transform.right, bobDistance, bobSpeed));
            yield return StartCoroutine(Move(-transform.right, bobDistance, bobSpeed * 3f));
        }
    }

    private IEnumerator Move(Vector3 direction, float distance, float  bobspeed)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + direction * distance;
        float elapsedTime = 0f;

        while (elapsedTime < distance / bobSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * bobspeed) / distance);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

}
