using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManagerWookie : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Controller controller;
    public Rigidbody2D rb;
    public PlayerManager playerManager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (controller.Grounded)
        {
            if (Input.GetKey(KeyCode.Space) && playerManager.getNumberOfHealth() > 0)
            {
                animator.Play("Charging");
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            else
                rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
        if (!controller.Grounded && playerManager.getNumberOfHealth() > 0)
        {
            animator.Play("jumpingUp");
        }
        if (animator.GetBool("walking") && !Input.GetKey(KeyCode.Space) && controller.Grounded && playerManager.getNumberOfHealth() > 0)
        {
            animator.Play("walking_anim");
        }
        if (!animator.GetBool("walking") && !Input.GetKey(KeyCode.Space) && controller.Grounded && playerManager.getNumberOfHealth() > 0)
        {
            animator.Play("Idle_anim");
        }
        if (playerManager.getNumberOfHealth() <= 0)
        {
            PlayerManager player = FindObjectOfType<PlayerManager>();
            player.GetComponent<Controller>().enabled = false;
            animator.Play("death");
        }
    }
    public void land()
    {
        animator.Play("landing");
    }

}
