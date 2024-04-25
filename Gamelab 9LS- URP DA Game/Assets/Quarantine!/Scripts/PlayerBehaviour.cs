using UnityEngine;
using UnityEngine.InputSystem;


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
        [SerializeField] private GameObject player1Model;
        [SerializeField] private GameObject player2Model; 

        private void Awake()
        {
            CC = GetComponent<CharacterController>();
        }

        private void Start()
        {
            heldAnimal = new Animal()
            {
                type = AnimalTypes.Empty,
                state = SickState.healthy, 
                sickProgression = 0
            };

            if (!QuarentineManager.Instance.playerOneSpawned)
            {
                QuarentineManager.Instance.playerOneSpawned = true;

                player1Model.SetActive(true);
                player2Model.SetActive(false);
                transform.position = GameObject.FindGameObjectWithTag("spawn1").transform.position;
                GameManager.Instance.playerBehaviour1 = this;
            }
            else
            {
                player1Model.SetActive(false);
                player2Model.SetActive(true);
                transform.position = GameObject.FindGameObjectWithTag("spawn2").transform.position;
                GameManager.Instance.playerBehaviour2 = this;
            }
            moveVector = Vector2.zero;
        }

        private void Update()
        {
            if (!QuarentineManager.Instance.PlayerSpawned() || QuarentineManager.Instance.GameOver()) return;

            if (sprintSpeed > 1f)
            {
                sprintSpeed -= Time.deltaTime * sprintBonus;
            }
            else if (sprintSpeed < 1f) { sprintSpeed = 1f; }

            ScanInteractable(gameObject, transform.forward, 3);
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

        public void UpdateHeldAnimal()
        {
            if (attachPoint.transform.childCount > 0) 
            {  
                Destroy(attachPoint.transform.GetChild(0).gameObject);
            }
            Instantiate(VisualManager.instance.GetAnimalVisuals(heldAnimal.type).model, attachPoint.transform);
        }

        private void FixedUpdate()
        {
            if (!QuarentineManager.Instance.PlayerSpawned() || QuarentineManager.Instance.GameOver()) return;

            if (moveVector.magnitude == 0) { return; }

            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);

            var targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVeloctiy, smoothtime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            CC.Move(movement * moveSpeed * sprintSpeed * Time.deltaTime);
        }
    }
}

