using UnityEngine;

namespace StackThatBulance
{
    public class CursorBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform grapPoint;
        [SerializeField] private float movespeed = 10f;

        public Vector2 moveVector = Vector2.zero;

        private GameObject grabTarget;
        private Rigidbody2D rb;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rb.velocity = moveVector * movespeed;

            if (grabTarget != null)
            {
                grabTarget.transform.position = grapPoint.transform.position;
            }
        }

        public void GrabTarget(GameObject target)
        {
            if (grabTarget == null)
            {
                grabTarget = target;
                grabTarget.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            else
            {
                grabTarget.GetComponent<Rigidbody2D>().gravityScale = 1;
                grabTarget = null;
            }
        }
    }
}

