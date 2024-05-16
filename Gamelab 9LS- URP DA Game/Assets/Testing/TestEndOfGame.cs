using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndOfGame : MonoBehaviour
{

    [Header("EndOfGameSequence")]
        private bool EndOfGameComplete = false;
    [SerializeField] private GameObject floatText;
    [SerializeField] private float floatOffset = 2f;

    [SerializeField] List<GameObject> Cages = new();

    private void Awake()
    {
        StartCoroutine(EndOfGame());

    }

    private IEnumerator EndOfGame()
    {
        float performance = 0;

        //float estMin = Cages.Count - cageQuota;
        //float estMax = Cages.Count;
        yield return new WaitForSeconds(0.1f);

        foreach (GameObject cage in Cages)
        {
            //CageBehaviour cageBehaviour = cage.GetComponent<CageBehaviour>();

            //if (cageBehaviour.myAnimal.state == SickState.healthy)
            {
                //performance += (1 - estMin) / (estMax - estMin) * 100;
                int AddedScore = Mathf.RoundToInt(performance);

                Vector3 textPos = cage.transform.position;
                textPos.y = cage.transform.position.y + floatOffset;


                GameObject floatTextInstance = Instantiate(floatText, textPos, cage.transform.rotation ,cage.transform);
                floatTextInstance.GetComponent<FloatText>().SetScore(AddedScore);
                //GameManager.Instance.IncreaseScore(AddedScore);
                yield return new WaitForSeconds(0.3f);

            }
        }
        EndOfGameComplete = true;


    }
}
