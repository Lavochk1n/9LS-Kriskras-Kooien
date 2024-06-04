using Quarantine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;
    private float difficulty = 100;
    private int ambulanceDepartures = 1;
    public PlayerNames playerNames; 
    
    public PlayerBehaviour playerBehaviour1, playerBehaviour2;
    

    [Header("difficulty sliders")]
    private float startDif = 1f; 
    [SerializeField][Range(1f, 5f)] private float maxDif = 2f;
    [SerializeField][Range(0.001f, 1f)] private float difficultyIncrease = .01f;
    [SerializeField][Range(1, 8)] private int maxGloves = 6;

    [Header ("SceneInitialization")]
    private bool levelIsTutorial = false;
    public bool randomGame = false;
    public int gameRoom;
    public bool flaggedMode= true; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerNames = new PlayerNames();
            playerNames.SaveNames("", "", "");
        }
        else
        {
            Destroy(gameObject);
        }
    }

   //////////// scene testing /////

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

    /////////// SCORE ////////

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetValues()
    {
        score = 0;
        ambulanceDepartures = 1; 
    }

    /////// Rules //////

    public void AddDeparture()
    {
        ambulanceDepartures ++;
    }

    public int GetDepartures()
    {
        return ambulanceDepartures;
    }

    public int GetMaxGloves()
    {
        return maxGloves;
    }

    /////////// Difficulty ///////////

    /// <summary>
    /// Calculates difficiculty ratio
    /// </summary>
    /// <returns> 
    /// Ratio between 1 and maxDif 
    /// </returns>
    public float GetDifficultyRatio()
    {
        difficulty = maxDif - (maxDif - startDif) * Mathf.Pow(System.MathF.E, -difficultyIncrease * ambulanceDepartures) ;
        Debug.Log(difficulty);
        return difficulty;
    }

    public class PlayerNames
    {
        public string Name1 { get; private set; }
        public string Name2 { get; private set; }
        public string TeamNAme { get; private set; }

        public void SaveNames(string name1, string name2, string teamName)
        {
            Name1 = name1;
            Name2 = name2;
            TeamNAme = teamName;
        }
    }

}


