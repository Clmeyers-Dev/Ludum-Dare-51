using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerBehavior : MonoBehaviour
{
    private MonsterManager monsterManager;
    private bool triggered = true;
    Rigidbody2D rb;
    [SerializeField]
    private float jumpForce;
    private Transform player;
    public bool storedGrounded;
    public float y;
    // Start is called before the first frame update
    void Start()
    {
        monsterManager = GetComponent<MonsterManager>();
        rb = monsterManager.rb;
        player = FindObjectOfType<PlayerManager>().transform;
        storedGrounded = monsterManager.getGrounded();
    }

    // Update is called once per frame
    void Update()
    {
        if (storedGrounded == false && monsterManager.getGrounded() == true)
        {
            landed();
            storedGrounded = monsterManager.getGrounded();
        }
        else
        {
            storedGrounded = monsterManager.getGrounded();
        }
        if (triggered)
            State();
    }
    void landed()
    {
        monsterManager.animator.Play("JumpDown_Anim");
     
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            triggered = true;
            
        }
       
    }
    void State()
    {

        if (monsterManager.getGrounded())
        {
           
            jump();
        }
        if (!monsterManager.getGrounded() && monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {
            monsterManager.animator.Play("air_anim");
        }

    }
    void jump()
    {


        if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {


            monsterManager.animator.Play("JumpUp_anim");
            if (transform.position.x < player.position.x)
            {
                rb.velocity = jumpForce * new Vector2(1, y); //for right jumping

            }
            else
            {
                rb.velocity = jumpForce * new Vector2(-1, y);
            }
        }
    }
     
}
