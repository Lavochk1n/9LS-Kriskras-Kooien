using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    public static PlayerConfigManager Instance { get; private set; }

    [SerializeField] private List<GameObject> hatPrefabs = new();

    [SerializeField] private List<GameObject> EmptyPanels = new();


    private List<PlayerConfig> playerConfigs;
    [SerializeField] private int maxPlayers = 2;


    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public List<GameObject> GetHatsConfigs()
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

    public void SetPlayerHat(int playerIndex,int hatIndex)
    {
        playerConfigs[playerIndex].Hat = hatPrefabs[hatIndex];
        Debug.Log(hatIndex);
    }

    public void ReadyPlayer(int playerIndex)
    {
        playerConfigs[playerIndex].isReady = true; 

        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true)) 
        {
            ScenesManager.Instance.NextScene();
        }
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
    public GameObject Hat {  get; set; }
}