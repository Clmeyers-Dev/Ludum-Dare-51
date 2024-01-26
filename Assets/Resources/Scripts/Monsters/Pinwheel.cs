using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pinwheel : MonoBehaviour
{
    public GameObject[] slices = new GameObject[8];
    public MonsterManager monster;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float flySpeed;
    public Vector3 start;
    [SerializeField]
    private Transform player;
    public bool returning;
    private float timetoReturn;
    [SerializeField]
    private float maxTimeToReturn;
    public bool canHit;
    public float sienfield;
    private float timeBtwHits;
    [SerializeField]
    private float maxBtwHits;
    [SerializeField]
    private float offset;
    // Start is called before the first frame update
    public float timeBtwPhase;
    public float maxTimeBtwPhase;
    [SerializeField]
    private Slider healthBar;
    public bool idle;
    void Start()
    {
        monster = GetComponent<MonsterManager>();
        start = transform.position;
        player = FindObjectOfType<PlayerManager>().transform;
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = monster.getCurrentHealth();
        healthBar.value = monster.getCurrentHealth();
    }

    // Update is called once per frame
    public bool inmelee;
    public bool inRanged;
    public int randomnumber;

    void Update()
    {
        healthBar.value = monster.getCurrentHealth();
        if (monster.getCurrentHealth() == 70)
        {

            Destroy(slices[6]);
        }
        if (monster.getCurrentHealth() == 60)
        {

            Destroy(slices[5]);
        }
        if (monster.getCurrentHealth() == 50)
        {

            Destroy(slices[4]);
        }
        if (monster.getCurrentHealth() == 40)
        {

            Destroy(slices[3]);
        }
        if (monster.getCurrentHealth() == 30)
        {

            Destroy(slices[2]);
        }
        if (monster.getCurrentHealth() == 20)
        {

            Destroy(slices[1]);
        }
        if (monster.getCurrentHealth() == 10)
        {

            Destroy(slices[0]);
        }


        if (timeBtwPhase < maxTimeBtwPhase)
        {
            timeBtwPhase += Time.deltaTime;
        }
        else
        {

            randomnumber = Random.Range(0, 3);
            if (randomnumber == 0)
            {
                idle = false;
                inmelee = true;
                inRanged = false;
                timeBtwPhase = 0;
            }
            if (randomnumber == 1)
            {
                idle = true;
                inmelee = false;
                inRanged = false;
                timeBtwPhase = maxTimeBtwPhase - 2;
            }
            if (randomnumber == 2)
            {
                inRanged = true;
                idle = false;
                inmelee = false;
                timeBtwPhase = 0;
            }


        }
        if (idle)
        {
            monster.animator.Play("Idle");

            monster.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
        if (inmelee)
        {
            spin();
            meleephase();
            if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monster.animator.IsInTransition(0))
                monster.animator.Play("spin");
            monster.rb.constraints = RigidbodyConstraints2D.None;
        }
        if (inRanged)
        {
            if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monster.animator.IsInTransition(0))
                monster.animator.Play("Fire");
            monster.rb.SetRotation(0);
            monster.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }
    void DisableMelee()
    {
        inmelee = false;
    }
    void meleephase()
    {
        if (timeBtwHits < maxBtwHits && !returning)
        {
            timeBtwHits += Time.deltaTime;
            fly();
        }
        else
        {
            canHit = true;
            swoop();
        }
        if (returning)
        {
            if (timetoReturn < maxTimeToReturn)
            {
                timetoReturn += Time.deltaTime;
                returnFly();
            }
            else
            {
                timetoReturn = 0;
                returning = false;

            }
        }
    }
    void fly()
    {
        sienfield = Mathf.Sin(Time.realtimeSinceStartup) * offset;
        if (Mathf.Sin(Time.realtimeSinceStartup) * offset < 0)
        {
            monster.rb.velocity = new Vector2(-1, 0) * flySpeed;
        }
        else
        {
            monster.rb.velocity = new Vector2(1, 0) * flySpeed;
        }

    }
    void shoot()
    {
        for (int i = 0; i < slices.Length; i++)
        {
            if (slices[i] != null)
                slices[i].GetComponent<sliceBehavior>().shoot();
        }
    }
    void spin()
    {
        monster.rb.rotation += rotateSpeed;
    }
    void returnFly()
    {
        Vector3 dir = (start - monster.rb.transform.position).normalized;
        if (Vector3.Distance(start, monster.rb.transform.position) > 1)
        {
            monster.rb.MovePosition(monster.rb.transform.position + dir * flySpeed * Time.fixedDeltaTime);
            canHit = false;
        }
        else
        {
            timetoReturn = maxTimeToReturn;
        }
    }

    void swoop()
    {
        Vector3 dir = (player.transform.position - monster.rb.transform.position).normalized;
        if (Vector3.Distance(player.transform.position, monster.rb.transform.position) > 0)
        {
            monster.rb.MovePosition(monster.rb.transform.position + dir * flySpeed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && canHit)
        {
            returning = true;
            canHit = false;
            PlayerManager pm = other.GetComponent<PlayerManager>();

            pm.LoseHealth();
            timeBtwHits = 0;
        }

    }
}