using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pinwheel : MonoBehaviour
{
    public GameObject[] slices = new GameObject[8];
    public MonsterManager monster;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float flySpeed;
    [SerializeField] private Transform player;

    public Vector3 start;
    private float timetoReturn;
    [SerializeField] private float maxTimeToReturn;
    public bool canHit;
    public float sienfield;
    private float timeBtwHits;
    [SerializeField] private float maxBtwHits;
    [SerializeField] private float offset;

    public float timeBtwPhase;
    public float maxTimeBtwPhase;

    [SerializeField] private Slider healthBar;
    public bool idle;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        healthBar.value = monster.getCurrentHealth();

        UpdateSlices();

        if (timeBtwPhase < maxTimeBtwPhase)
        {
            timeBtwPhase += Time.deltaTime;
        }
        else
        {
            ChoosePhase();
        }

        if (idle)
        {
            HandleIdle();
        }

        if (monster.inMelee)
        {
            HandleMeleePhase();
        }

        if (monster.inRanged)
        {
            HandleRangedPhase();
        }
    }

    void Initialize()
    {
        monster = GetComponent<MonsterManager>();
        start = transform.position;
        player = FindObjectOfType<PlayerManager>().transform;
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = monster.getCurrentHealth();
        healthBar.value = monster.getCurrentHealth();
    }

    void UpdateSlices()
    {
        for (int i = 0; i < slices.Length; i++)
        {
            if (monster.getCurrentHealth() == (i + 1) * 10)
            {
                Destroy(slices[i]);
            }
        }
    }

    void ChoosePhase()
    {
        int randomnumber = Random.Range(0, 3);
        if (randomnumber == 0)
        {
            idle = false;
            monster.inMelee = true;
            monster.inRanged = false;
            timeBtwPhase = 0;
        }
        else if (randomnumber == 1)
        {
            idle = true;
            monster.inMelee = false;
            monster.inRanged = false;
            timeBtwPhase = maxTimeBtwPhase - 2;
        }
        else if (randomnumber == 2)
        {
            monster.inRanged = true;
            idle = false;
            monster.inMelee = false;
            timeBtwPhase = 0;
        }
    }

    void HandleIdle()
    {
        monster.animator.Play("Idle");
        monster.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    void HandleMeleePhase()
    {
        spin();
        meleephase();

        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monster.animator.IsInTransition(0))
        {
            monster.animator.Play("spin");
        }
        monster.rb.constraints = RigidbodyConstraints2D.None;
    }

    void HandleRangedPhase()
    {
        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monster.animator.IsInTransition(0))
        {
            monster.animator.Play("Fire");
        }
        monster.rb.SetRotation(0);
        monster.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    void spin()
    {
        monster.rb.rotation += rotateSpeed;
    }

    void meleephase()
    {
        if (timeBtwHits < maxBtwHits && !monster.returning)
        {
            timeBtwHits += Time.deltaTime;
            fly();
        }
        else
        {
            canHit = true;
            swoop();
        }

        if (monster.returning)
        {
            if (timetoReturn < maxTimeToReturn)
            {
                timetoReturn += Time.deltaTime;
                returnFly();
            }
            else
            {
                timetoReturn = 0;
                monster.returning = false;
            }
        }
    }

    void fly()
    {
        sienfield = Mathf.Sin(Time.realtimeSinceStartup) * offset;
        Vector2 velocity = Mathf.Sin(Time.realtimeSinceStartup) * offset < 0 ? new Vector2(-1, 0) * flySpeed : new Vector2(1, 0) * flySpeed;
        monster.rb.velocity = velocity;
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
            monster.returning = true;
            canHit = false;
            PlayerManager pm = other.GetComponent<PlayerManager>();
            pm.loseHealth();
            timeBtwHits = 0;
        }
    }
}
