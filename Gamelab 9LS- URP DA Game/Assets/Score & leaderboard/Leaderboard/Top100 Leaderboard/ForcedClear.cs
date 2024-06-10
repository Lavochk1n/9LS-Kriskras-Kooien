using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedClear : MonoBehaviour
{
    // Start is called before the first frame update
    public void Clear()
    {
        HighscoresManager.Instance.ClearHighScores();
    }
}
