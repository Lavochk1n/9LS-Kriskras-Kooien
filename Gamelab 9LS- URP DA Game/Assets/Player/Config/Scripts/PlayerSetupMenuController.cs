using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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

    private Vector2 rotationInput;
    [SerializeField] float rotSpeed = 2f; 

    public RawImage rawImage;

    private PlayerConfigManager pcm; 

    private void Awake()
    {
        controls = new StandardPlayerInput();
        pcm = PlayerConfigManager.Instance;

    }


    public void SetPlayerIndex(PlayerInput pi)
    {
        Input = pi;
        PlayerIndex = pi.playerIndex;
        titleText.SetText("Speler " + (PlayerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
        Input.onActionTriggered += OnInput;

    }

    private void Start()
    {
        currentHat = pcm.CycleHat(PlayerIndex, 1);
        DisplayHat();
    }
    private void OnDisable()
    {
        Input.onActionTriggered -= OnInput;
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
        RotateCharacter();
    }


    private void OnInput(InputAction.CallbackContext cbc)
    {
        if (!inputEnabled) { return; }

        if (!cycleEnabled) { return; }

        if (!menuPanel.activeInHierarchy || menuPanel == null) { return; }

        if (cbc.action.name == controls.Player.Rotate.name)
        {
            RotateChar(cbc);
        }

        if (cbc.action.name == controls.Player.Movement.name)
        {
            CycleHat(cbc);
        }

        
    }

    private void RotateChar(InputAction.CallbackContext cbc)
    {
        rotationInput = cbc.ReadValue<Vector2>();
    }

    private void RotateCharacter()
    {
        if (rotationInput == Vector2.zero) return;

        float horizontalInput = rotationInput.x;
        float movementThreshold = 0.5f;

        // the gods will pusnish me for this line 
        Transform player = GameObject.FindGameObjectWithTag("spawn" + (PlayerIndex + 1).ToString()).transform.parent.parent.parent.parent.parent.parent;

        if (Mathf.Abs(horizontalInput) > movementThreshold)
        {
            Vector3 rotation = Vector3.up * horizontalInput * rotSpeed * Time.deltaTime;
            player.Rotate(rotation);
        }
    }

    private void CycleHat(InputAction.CallbackContext cbc)
    {
        float horizontalInput = cbc.ReadValue<Vector2>().x;

        float movementThreshold = 0.5f;

        if (Mathf.Abs(horizontalInput) > movementThreshold)
        {
            if (horizontalInput > 0)
            {
                currentHat = pcm.CycleHat(PlayerIndex, 1);
                //currentHat += 1;
                //if (currentHat >= PlayerConfigManager.Instance.GetHatsConfigs().Count)
                //{
                //    currentHat = 0;
                //}
            }
            else
            {
                currentHat = pcm.CycleHat(PlayerIndex, -1);

                //currentHat -= 1;
                //if (currentHat < 0)
                //{
                //    currentHat = PlayerConfigManager.Instance.GetHatsConfigs().Count -1;
                //}
            }
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
            PlayerConfigManager.Instance.GetHatsConfigs()[currentHat].model, 
            hatPoint.transform.position, 
            hatPoint.transform.rotation, 
            hatPoint.transform);
    }

    public void SetHat()
    {
        if (!inputEnabled) { return; }

        PlayerConfigManager.Instance.SetPlayerHat(PlayerIndex, currentHat);
        readyPanel.SetActive(true);
        menuPanel.SetActive(false);
    }


    public void ReadyPlayer()
    {
        if(!inputEnabled) { return; }

        PlayerConfigManager.Instance.ReadyPlayer(PlayerIndex);
    }
}
