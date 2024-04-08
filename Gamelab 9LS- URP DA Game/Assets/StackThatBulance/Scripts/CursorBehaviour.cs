using UnityEngine;

namespace StackThatBulance
{
    public class CursorBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform grapPoint; 
        [SerializeField] private float movespeed = 10f;

        public Vector3 moveVector = Vector3.zero;

        private GameObject grabTarget;
        private Rigidbody rb;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
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
                grabTarget.GetComponent<Rigidbody>().useGravity = false;
            }
            else
            {
                grabTarget.GetComponent<Rigidbody>().useGravity = true;
                grabTarget = null;
            }
        }
    }
}