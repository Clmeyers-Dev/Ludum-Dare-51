using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    ParticleSystem deathParticles;
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
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        checkGrounded();
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
        currentHealth -= damage;
    }
    public void die()
    {
        if (currentHealth <= 0)
        {
            ParticleSystem Particle = Instantiate(deathParticles, transform);
            Particle.Play();
            Destroy(this.gameObject);
        }
    }
}
