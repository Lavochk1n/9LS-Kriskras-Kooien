using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;


namespace Quarantine
{
    public class PlayerBehaviour : Interactor
    {
        private Vector2 moveVector;
        public float moveSpeed = 5f;

        private CharacterController CC;

        public Animal heldAnimal;



        [SerializeField] private float smoothtime = 0.5f, currentVeloctiy;

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
        }


        private void Update()
        {

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


        private void FixedUpdate()
        {
            if (moveVector.magnitude == 0) { return; }

            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);

            var targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVeloctiy, smoothtime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            CC.Move(movement * moveSpeed * Time.deltaTime);



        }
    }
}

