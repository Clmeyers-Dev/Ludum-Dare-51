using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    public GameObject deathParticles;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public Rigidbody2D rb;
    private float dazedTime;
    public float startDazedTime;
    [SerializeField]
    private string idleAnimation;
    [SerializeField]
    private bool stationary;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (dazedTime <= 0)
        {
            //return to normal
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            animator.Play(idleAnimation);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            dazedTime -= Time.deltaTime;
        }
        if(!stationary){
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        }
        checkGrounded();
        if (currentHealth <= 0)
            die();
    }
    void checkGrounded()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .15f, whatIsGround);
    }

    public bool getGrounded()
    {
        return isGrounded;
    }
    public void takeDamage(float damage)
    {
        dazedTime = startDazedTime;
        currentHealth -= damage;
    }
    public void die()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathParticles, transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
