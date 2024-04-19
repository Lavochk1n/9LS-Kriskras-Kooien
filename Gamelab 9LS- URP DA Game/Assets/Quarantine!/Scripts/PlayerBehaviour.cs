using UnityEngine;
using UnityEngine.InputSystem;


namespace Quarantine
{
    public class PlayerBehaviour : Interactor
    {
        [SerializeField] private Color color1, color2; 

        private Vector2 moveVector;

        public float moveSpeed = 5f;

        private float sprintSpeed = 1f;
        [SerializeField] private float sprintBonus = 3f; 

        private CharacterController CC;

        public Animal heldAnimal;

        [SerializeField] private float smoothtime = 0.5f, currentVeloctiy;


        [SerializeField] private GameObject parrotModel, crowModel, dogModel;

        [SerializeField] private GameObject player1Model, player2Model; 

        private void Awake()
        {
            CC = GetComponent<CharacterController>();

        }

        private void Start()
        {
            heldAnimal = new Animal()
            {
                type = animalTypes.Empty,
                state = sickState.healthy, 
                sickProgression = 0
            };

            if (!MiniGameManager.Instance.playerOneSpawned)
            {
                MiniGameManager.Instance.playerOneSpawned = true;

                player1Model.SetActive(true);
                player2Model.SetActive(false);
                transform.position = GameObject.FindGameObjectWithTag("spawn1").transform.position;
                MiniGameManager.Instance.inventory1.player = this;
                MiniGameManager.Instance.inventory1.SetColour(color1);



            }
            else
            {


                player1Model.SetActive(false);
                player2Model.SetActive(true);
                transform.position = GameObject.FindGameObjectWithTag("spawn2").transform.position;
                MiniGameManager.Instance.inventory2.player = this;
                MiniGameManager.Instance.inventory2.SetColour(color2);


            }
            moveVector = Vector2.zero;



        }


        private void Update()
        {

            if (!MiniGameManager.Instance.PlayerSpawned() || MiniGameManager.Instance.GameOver()) return;

            if (sprintSpeed > 1f)
            {
                sprintSpeed -= Time.deltaTime * sprintBonus;
            }
            else if (sprintSpeed < 1f) { sprintSpeed = 1f; }



            ScanInteractable(gameObject, transform.forward, 3);


            UpdateHeldAnimal();

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



        ///  MAKE THIS HAPPPEM ON EVENT FOR PERFORMANCE 
        private void UpdateHeldAnimal()
        {
            switch (heldAnimal.type)
            {
                case animalTypes.dog:
                    dogModel.SetActive(true);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(false);
                    break;
                case animalTypes.crow:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(true);
                    break;
                case animalTypes.parrot:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(true);
                    crowModel.SetActive(false);
                    break;
                case animalTypes.Empty:
                    dogModel.SetActive(false);
                    parrotModel.SetActive(false);
                    crowModel.SetActive(false);
                    break; 
            }
        }

        private void FixedUpdate()
        {

            if (!MiniGameManager.Instance.PlayerSpawned() || MiniGameManager.Instance.GameOver()) return;

            if (moveVector.magnitude == 0) { return; }

            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);

            var targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVeloctiy, smoothtime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            CC.Move(movement * moveSpeed * sprintSpeed * Time.deltaTime);



        }
    }
}

