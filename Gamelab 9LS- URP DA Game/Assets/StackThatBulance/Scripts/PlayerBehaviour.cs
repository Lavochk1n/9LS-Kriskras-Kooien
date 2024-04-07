using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StackThatBulance
{
    public class PlayerBehaviour : MonoBehaviour
    {


        private CursorBehaviour CursorMovement;

        public bool playerOne;

        private void Awake()
        {

            CursorMovement = GameObject.FindWithTag("Cursor").GetComponent<CursorBehaviour>();
            
            if(!GameManager.Instance.playerOneSpawned)
            {
                playerOne = true;
                GameManager.Instance.playerOneSpawned = true;
            }
            else
            {
                playerOne = false;
            }
        }



        public void OnMovement(InputAction.CallbackContext value)
        {

            if (!playerOne)
            {
                CursorMovement.moveVector.x = value.ReadValue<Vector2>().x;
            }
            else
            {
                CursorMovement.moveVector.z = value.ReadValue<Vector2>().y;

            }
        }


        public void OnGrab()
        {
            CursorMovement.AttemptGrab();
        }
    }

    
}