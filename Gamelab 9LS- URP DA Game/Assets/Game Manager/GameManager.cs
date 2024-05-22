using Quarantine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;
    private float difficulty = 100;


    [Header("difficulty sliders")]

    [SerializeField][Range(50.0f, 200.0f)] private float newGameDifficulty = 100f;
    [SerializeField][Range(1f, 1.50f)] private float difficultyIncrease = 1.1f;
    [SerializeField][Range(1, 24)] private int ambulanceDepartures = 6;
    [SerializeField][Range(1, 8)] private int maxGloves = 6;


    public PlayerBehaviour playerBehaviour1, playerBehaviour2;

    [Header ("SceneInitialization")]
    private bool levelIsTutorial = false;

    public bool randomGame = false;
    public int gameRoom;

    //public AmbulanceBehaviour AM;

    public bool flaggedMode= true; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    ////////////////////////////////// scene testing ////////////////////////////

    public void SetTutorial(bool state)
    {
        levelIsTutorial=state;
    }

    public bool IsTutorial()
    {
        return levelIsTutorial;
    }

    public void DetermineScene(int scene)
    {
        gameRoom = scene;
    }

    ////////////////////////////////// SCORE ////////////////////////////

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }

    ////////////////////////////////// Rules ////////////////////////////

    public int GetTotalDepartures()
    {
        return ambulanceDepartures;
    }

    public int GetMaxGloves()
    {
        return maxGloves;
    }

    ////////////////////////////////// Difficulty ////////////////////////////


    public float IncreaseDifficulty()
    {
        return difficulty *= difficultyIncrease;
    }

    public float GetDifficultyRatio()
    {
        return difficulty/100;
    }

    
    

    public void Reset()
    {
        score = 0;
        difficulty = newGameDifficulty;
    }
}
