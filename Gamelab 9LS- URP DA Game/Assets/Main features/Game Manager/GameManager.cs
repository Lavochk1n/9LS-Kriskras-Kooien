using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score = 0;
    private float timeLeft = 60f;
    private float difficulty = 100; 


    [SerializeField] private float newGameTime = 210f;
    [SerializeField] private float newGameDifficulty = 100f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }


    public void SetTimeLeft(float amount)
    {
        timeLeft = amount;
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }


    public float IncreaseDifficulty(float amount)
    {
        return difficulty += amount;
    }

    public float GetDifficultyRatio()
    {
        return difficulty/100;
    }


    public void Reset()
    {
        score = 0;
        difficulty = newGameDifficulty;
        timeLeft = newGameTime; 
    }
}
