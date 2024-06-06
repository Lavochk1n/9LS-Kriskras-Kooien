using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.InputSystem.InputAction;


namespace Quarantine
{
    public class PlayerBehaviour : Interactor
    {
        [Header("Player Movement")]
        public float moveSpeed = 5f;
        [SerializeField] private float sprintBonus = 3f; 
        [SerializeField] private float smoothtime = 0.5f, currentVeloctiy;

        private CharacterController CC;
        private Vector2 moveVector;
        private float sprintSpeed = 1f;

        [Header("Holding Animal")]
        public Animal heldAnimal;
        [SerializeField] private GameObject attachPoint;

        [Header("Player Distinction")]
        [SerializeField] private GameObject hatPoint;
        [SerializeField] private GameObject player1model, player2model;
        public HatVisuals hatvisuals; 


        private PlayerConfig playerConfig;
        private StandardPlayerInput controls;


        public int maxFlags = 2; 
        public int flagAmount;

        public CageBehaviour mostRecentCage;

        public GameObject myUI;

        public bool isHolding; 

        private void Awake()
        {
            CC = GetComponent<CharacterController>();
            controls = new StandardPlayerInput();

        }


        public void InitializePlayer(PlayerConfig pc)
        {
            flagAmount = maxFlags; 
            playerConfig = pc;
            Debug.Log(pc.PlayerIndex.ToString() + pc.Hat.ToString());
            hatvisuals = pc.Hat;



            if (pc.PlayerIndex == 0)
            {
               
                transform.position = GameObject.FindGameObjectWithTag("spawn1").transform.position;
                GameManager.Instance.playerBehaviour1 = this;
                player1model.SetActive(true);
                player2model.SetActive(false);
            }
            else
            {
           
                transform.position = GameObject.FindGameObjectWithTag("spawn2").transform.position;
                GameManager.Instance.playerBehaviour2 = this;
                player1model.SetActive(false);
                player2model.SetActive(true);
            }


            Instantiate(hatvisuals.model, hatPoint.transform.position, hatPoint.transform.rotation, hatPoint.transform);

            playerConfig.Input.onActionTriggered += Input_onActionTriggered;

        }

        private void Input_onActionTriggered(InputAction.CallbackContext obj)
        {
            if (obj.action.name == controls.Player.Movement.name)
            {
                OnMovementPerformed(obj);
            }
            if (obj.action.name == controls.Player.Interact.name)
            {
                OnInteract(obj);
            }
            if (obj.action.name == controls.Player.Return.name)
            {
                OnReturn(obj);
            }
            if (obj.action.name == controls.Player.SecondairyInteract.name)
            {
                OnSecondairyInteract(obj);
            }
            if (obj.action.name == controls.Player.Pause.name)
            {
                OnPause(obj);
            }

        }

        private void Start()
        {
            heldAnimal = new Animal()
            {
                type = AnimalTypes.Empty,
                state = SickState.healthy, 
                sickProgression = 0
            };

       
            moveVector = Vector2.zero;

        }

        private void Update()
        {
            if (!QuarentineManager.Instance.PlayerSpawned() || QuarentineManager.Instance.GamePaused()) return;

            if (sprintSpeed > 1f)
            {
                sprintSpeed -= Time.deltaTime * sprintBonus;
            }
            else if (sprintSpeed < 1f) { sprintSpeed = 1f; }

            ScanInteractable(gameObject, transform.forward, 1);
        }

        public void OnMovementPerformed(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                RequestInteraction();
                isHolding = true; 
            }
            else if (context.canceled) 
            {
                RequestHold();
                isHolding = false;
            }
        }

        public void OnSecondairyInteract(InputAction.CallbackContext context)
        {
            if(GameManager.Instance.flaggedMode)
            {
                if (context.started)
                {
                    RequestRemove();
                }
            }
        }

        public void OnReturn(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (sprintSpeed <= 1f)
                {
                    sprintSpeed = sprintBonus;
                }
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                QuarentineManager.Instance.PauseGame();
            }
        }

        public void UpdateHeldAnimal()
        {
            if (attachPoint.transform.childCount > 0) 
            {  
                Destroy(attachPoint.transform.GetChild(0).gameObject);
            }
            Instantiate(VisualManager.instance.GetAnimalVisuals(heldAnimal.type).model, attachPoint.transform);
            myUI.GetComponent<InventoryUI>().UpdateInventoryUI(heldAnimal); 
        }

        private void FixedUpdate()
        {
            if (!QuarentineManager.Instance.PlayerSpawned() || QuarentineManager.Instance.GamePaused()) return;

            if (moveVector.magnitude == 0) { return; }

            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);

            var targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVeloctiy, smoothtime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            CC.Move(movement * moveSpeed * sprintSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            
                GameManager.Instance.playerBehaviour1 = null;
           
                GameManager.Instance.playerBehaviour2 = null;
            
        }
    }
}

