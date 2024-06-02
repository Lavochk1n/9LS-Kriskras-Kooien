
 using UnityEditor;
using UnityEngine;
public class ScoreEditorButtons : MonoBehaviour
{
    [MenuItem("Tools/Clear High Scores")]
    public static void ClearHighScores()
    {
        PlayerPrefs.DeleteKey("HighScoreTable");
        PlayerPrefs.Save();
        Debug.Log("High scores cleared.");
    }

}

