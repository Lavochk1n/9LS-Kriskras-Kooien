using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    public static PlayerConfigManager Instance { get; private set; }

    [SerializeField] private List<HatVisuals> hatPrefabs = new();

    [SerializeField] private List<GameObject> EmptyPanels = new();


    private List<PlayerConfig> playerConfigs;
    [SerializeField] private int maxPlayers = 2;


    private int player0Hat = -2;
    private int player1Hat = -2;

    

    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void ResetConfigs()
    {

        foreach (Transform player in transform)
        {
            Destroy(player.gameObject); 
        }
        playerConfigs.Clear();
    
    }

    public List<HatVisuals> GetHatsConfigs()
    {
        return hatPrefabs;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(Instance);
        playerConfigs = new List<PlayerConfig>();

    }


    

    public int CycleHat(int playerIndex, int indexChange)
    {
        int hat;
        int othersHat;

        AudioManager.Instance.PlaySFX(0);

        if (playerIndex == 0)
        {
            hat = player0Hat;
            othersHat = player1Hat;
        }
        else
        {
            hat = player1Hat;
            othersHat = player0Hat;
        }

        hat += indexChange;

        int hatCount = hatPrefabs.Count;
        if (hatCount <= 1) return hat;  

        while (true)
        {
            if (hat >= hatCount)
            {
                hat = 0;
            }
            else if (hat < 0)
            {
                hat = hatCount - 1;
            }

            if (hat == othersHat)
            {
                hat += indexChange > 0 ? 1 : -1;
            }
            else
            {
                break;
            }
        }

        if (playerIndex == 0)
        {
            player0Hat = hat;
        }
        else
        {
            player1Hat = hat;
        }

        return hat;
    }


    public void SetPlayerHat(int playerIndex,int hatIndex)
    {
        playerConfigs[playerIndex].Hat = hatPrefabs[hatIndex];

        Debug.Log(hatIndex);
    }

    public void ReadyPlayer(int playerIndex)
    {
        AudioManager.Instance.PlaySFX(2);

        playerConfigs[playerIndex].isReady = true;


        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true)) 
        {
            ScenesManager.Instance.RandomGame();
        }
    }

    public void UnreadyPlayer(int playerIndex)
    {
        playerConfigs[playerIndex].isReady = false;

    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player Joined " + pi.playerIndex);

        if (EmptyPanels.Count != 0)
        {
            Destroy(EmptyPanels[0]);
            EmptyPanels.RemoveAt(0);
        }

        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfig(pi)); 
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}

public class PlayerConfig
{
    public PlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get;  set; }
    public int PlayerIndex {  get;  set; }
    public bool isReady {  get;  set; }
    public HatVisuals Hat {  get; set; }
}