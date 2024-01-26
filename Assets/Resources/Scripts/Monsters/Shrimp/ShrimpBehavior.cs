using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform player;
    bool canHit;
    private float timeBtwHits;
    [SerializeField]
    private float maxBtwHits;
    [SerializeField]
    private float offset;
    public Vector3 start;
    public MonsterManager Monster;
    public Vector3 returnLocation;
    [SerializeField]
    private float speed;
    private float timetoReturn;
    [SerializeField]
    private float maxTimeToReturn;
    public bool returning;
    void Awake()
    {
        returnLocation = transform.position;
    }
    void Start()
    {
        start = transform.position;
        player = FindObjectOfType<PlayerManager>().transform;
    }
    // Update is called once per frame
    void Update()
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
    void returnFly()
    {
        Vector3 dir = (start - Monster.rb.transform.position).normalized;
        if (Vector3.Distance(start, Monster.rb.transform.position) > 1)
        {
            Monster.rb.MovePosition(Monster.rb.transform.position + dir * speed * Time.fixedDeltaTime);
            canHit =false;
        }else{
            timetoReturn = maxTimeToReturn;
        }
    }

    void swoop()
    {
        Vector3 dir = (player.transform.position - Monster.rb.transform.position).normalized;
        if (Vector3.Distance(player.transform.position, Monster.rb.transform.position) > 0)
        {
            Monster.rb.MovePosition(Monster.rb.transform.position + dir * speed * Time.fixedDeltaTime);
        }
    }
    public float sienfield;
    void fly()
    {
        sienfield = Mathf.Sin(Time.realtimeSinceStartup) * offset;
        if (Mathf.Sin(Time.realtimeSinceStartup) * offset < 0)
        {
            Monster.rb.velocity = new Vector2(-1, 0) * speed;
        }
        else
        {
            Monster.rb.velocity = new Vector2(1, 0) * speed;
        }

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(start, new Vector3(1, 1, 1));
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
