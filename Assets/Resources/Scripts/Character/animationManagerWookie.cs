using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManagerWookie : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Controller controller;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerManager playerManager;

    void Update()
    {
        if (playerManager.GetNumberOfHealth() > 0)
        {
            if (controller.Grounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    animator.Play("Charging");
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                    if (animator.GetBool("walking") && !Input.GetKey(KeyCode.Space))
                        animator.Play("walking_anim");
                    else if (!animator.GetBool("walking"))
                        animator.Play("Idle_anim");
                }
            }
            else
            {
                animator.Play("jumpingUp");
            }
        }
        else
        {
            PlayerManager player = FindObjectOfType<PlayerManager>();
            player.GetComponent<Controller>().enabled = false;
            animator.Play("death");
        }
    }

    public void Land()
    {
        animator.Play("landing");
    }
}
