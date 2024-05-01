using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph;
using Quarantine;

public class PlayerSetupMenuController : MonoBehaviour
{

    private int PlayerIndex;

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private GameObject readyPanel, menuPanel;

    [SerializeField] private Button readyButton;

    [SerializeField] private GameObject hatPoint; 

    private float ignoreInputTime = 1f, ignoreCycleTime = 0.3f, timer = 0f;
    private bool inputEnabled, cycleEnabled; 

    private int currentHat = 0;

    private PlayerInput Input;

    private StandardPlayerInput controls;

    private void Awake()
    {
        controls = new StandardPlayerInput();

    }


    public void SetPlayerIndex(PlayerInput pi)
    {
        Input = pi;
        PlayerIndex = pi.playerIndex;
        titleText.SetText("Player " + (PlayerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
        Input.onActionTriggered += OnMove;

    }


    private void OnDisable()
    {
        Input.onActionTriggered -= OnMove;
    }

    void Update()
    {
        if (Time.time > ignoreInputTime) 
        { 
            inputEnabled = true;
        }

        if (Time.time > timer)
        {
            cycleEnabled = true;
        }
    }


    private void OnMove(InputAction.CallbackContext cbc)
    {
        if (!inputEnabled) { return; }

        if(!cycleEnabled) {  return; }

        if (!menuPanel.activeInHierarchy || menuPanel == null) { return; }

        if (cbc.action.name != controls.Player.Movement.name)
        {
            return; 
        }

        float horizontalInput = cbc.ReadValue<Vector2>().x;

        float movementThreshold = 0.5f;

        if (Mathf.Abs(horizontalInput) > movementThreshold)
        {
            if (horizontalInput > 0)
            {
                currentHat += 1;
                if (currentHat >= PlayerConfigManager.Instance.GetHatsConfigs().Count)
                {
                    currentHat = 0;
                }
            }
            else
            {
                currentHat -= 1;
                if (currentHat < 0)
                {
                    currentHat = PlayerConfigManager.Instance.GetHatsConfigs().Count -1;
                }
            }
            Debug.Log(currentHat);
            timer = Time.time + ignoreCycleTime;
            cycleEnabled = false; 
        }

        DisplayHat(); 
    }
    

    private void DisplayHat()
    {

        hatPoint = GameObject.FindGameObjectWithTag("spawn" + (PlayerIndex + 1).ToString());

        if (hatPoint.transform.childCount > 0)
        {
            foreach (Transform child in hatPoint.transform)
            {
                Destroy(child.gameObject);
            }
        }
        Instantiate(
            PlayerConfigManager.Instance.GetHatsConfigs()[currentHat], 
            hatPoint.transform.position, 
            hatPoint.transform.rotation, 
            hatPoint.transform);
    }

    public void SetHat()
    {
        if (!inputEnabled) { return; }

        PlayerConfigManager.Instance.SetPlayerHat(PlayerIndex, currentHat);
        readyPanel.SetActive(true);
        readyButton.Select(); 
        menuPanel.SetActive(false);
    }


    public void ReadyPlayer()
    {
        if(!inputEnabled) { return; }

        PlayerConfigManager.Instance.ReadyPlayer(PlayerIndex); 
        readyButton.gameObject.SetActive(false);             
    }
}
