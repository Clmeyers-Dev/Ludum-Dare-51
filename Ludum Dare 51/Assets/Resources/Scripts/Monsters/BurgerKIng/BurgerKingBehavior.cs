using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BurgerKingBehavior : MonoBehaviour
{
    private MonsterManager monsterManager;
    private bool triggered = true;
    Rigidbody2D rb;
    [SerializeField]
    private float jumpForce;
    private Transform player;
    public bool storedGrounded;
    [SerializeField]
    private float fireangle;
    public float timeBtwSpawns;
    public float maxSpawnBtw;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject BurgerSpawn;
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private Slider healthBar;
    public float y;
    // Start is called before the first frame update
     private void Awake()
    {
         monsterManager = GetComponent<MonsterManager>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = monsterManager.getCurrentHealth();
        healthBar.value = monsterManager.getCurrentHealth();
    }
    void Start()
    {
       
        rb = monsterManager.rb;
        player = FindObjectOfType<PlayerManager>().transform;
        storedGrounded = monsterManager.getGrounded();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = monsterManager.getCurrentHealth();
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
        //monsterManager.animator.Play("JumpDown_Anim");

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            triggered = true;

        }

    }



    void spawn()
    {
        timeBtwSpawns = 0;
        var shot = Instantiate(BurgerSpawn, firePoint.position, Quaternion.identity);
        monsterManager.animator.Play("Idle");
    }
    void State()
    {
        if (timeBtwSpawns < maxSpawnBtw)
        {
            timeBtwSpawns += Time.deltaTime;

        }
        else
        {
            if (monsterManager.getGrounded())
                spawn();
        }

        if (monsterManager.getGrounded() && monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {

            jump();
        }
        if (!monsterManager.getGrounded() && monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {
            monsterManager.animator.Play("MidAir");
            var dif = player.position - transform.position;
            if (dif.magnitude > 1)
            {
                monsterManager.rb.AddForce(dif * trackingNumber * Time.deltaTime);
            }
        }

    }
    public float trackingNumber;
    void jump()
    {


        if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {


            monsterManager.animator.Play("JumpUp");
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
