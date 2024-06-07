using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUI : MonoBehaviour
{
    public float targetScale = 2.0f; // The target scale factor
    public float duration = 0.3f; // Duration over which to scale

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        if (image != null)
        {
            //Debug.Log("Image component found. Starting coroutine.");
            StartCoroutine(ScaleImageOverTime(targetScale, duration));
        }
        else
        {
            //Debug.LogError("No Image component found on this GameObject.");
        }
    }

    // Coroutine to scale the UI Image over time with ease-out effect
    IEnumerator ScaleImageOverTime(float target, float duration)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(target, target, target);
        float time = 0;

        while (time < duration)
        {
            // Calculate the eased progress
            float t = time / duration;
            t = 1f - Mathf.Pow(1f - t, 3); // Ease-out interpolation


            Color color = image.color;
            color.a = duration + 0.5f - time;
            image.color = color;
            // Scale the object
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            //Debug.Log($"Scaling... Time: {time}, Scale: {transform.localScale}");

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set correctly
        transform.localScale = targetScale;
        //Debug.Log("Final Scale applied. Scale: " + transform.localScale);

        // Destroy the GameObject after scaling is complete
        Destroy(gameObject);
        //Debug.Log("GameObject destroyed.");
    }
}