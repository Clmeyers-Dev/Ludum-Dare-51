using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class animationManagerWookie : MonoBehaviour
    {
        // Start is called before the first frame update
        public Animator animator;
        public Controller controller;
        public Rigidbody2D rb;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
            if (controller.Grounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    animator.Play("Charging");
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                }
                else
                    rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            }
            if (!controller.Grounded)
            {
                animator.Play("jumpingUp");
            }
            if (animator.GetBool("walking") && !Input.GetKey(KeyCode.Space) && controller.Grounded)
            {
                animator.Play("walking_anim");
            }
            if (!animator.GetBool("walking") && !Input.GetKey(KeyCode.Space) && controller.Grounded)
            {
                animator.Play("Idle_anim");
            }
        }
        public void land(){
            animator.Play("landing");
        }

    }
}