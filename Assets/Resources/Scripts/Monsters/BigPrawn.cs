using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BigPrawn : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;
    private MonsterManager monsterManager;
    public bool sunk;
    public float sinktime;
    public float maxSinkTime;
    public float laserTime;
    public float MaxLasterTime;

    public Vector3 offset;
    private MonsterManager Monster;

    [SerializeField]
    private float timer;
    public Transform firepoint;
    [SerializeField]
    private GameObject bullet;
    public GameObject parent;
        [SerializeField]
    private float fireangle;
    [SerializeField]
    public float fireSpeed = 0.5f;
     public bool knockback;

    private void Awake()
    {

        monsterManager = GetComponentInChildren<MonsterManager>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = monsterManager.getCurrentHealth();
        healthBar.value = monsterManager.getCurrentHealth();

    }
    // Start is called before the first frame update
    private Transform spawnLocation;
    void Start()
    {
        Monster = GetComponent<MonsterManager>();
        spawnLocation = FindObjectOfType<pawnSpawn>().transform;
        transform.position = spawnLocation.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = monsterManager.getCurrentHealth();
        if (sunk && laserTime < MaxLasterTime)
        {
            laserTime += Time.deltaTime;
            if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
                monsterManager.animator.Play("Laser");

        }
        else
        {
            rise();
        }
        if (!sunk && sinktime < maxSinkTime)
        {
            sinktime += Time.deltaTime;
        }
        else
        {

            if (!flipped)
            {
                if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
                    monsterManager.animator.Play("FlipOff");

            }
            if (flipped && !sunk)
            {
                sink();
            }

        }

    }

    public bool flipped;
    void shootLaser()
    {
        var shot = Instantiate(bullet, firepoint.position, Quaternion.Euler(parent.transform.localEulerAngles.x, parent.transform.localEulerAngles.y, parent.transform.localEulerAngles.z + fireangle));
    }
    void knockAway()
    {
        knockback =true;
    }
    void noKnockBack(){
        knockback = false;
    }
    public float speed;
    void sink()
    {
        if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
            monsterManager.animator.Play("Sink");
        if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {
            sunk = true;
            sinktime = 0;
        }
    }
    public float offset2;
    void rise()
    {
        if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
            monsterManager.animator.Play("Rise");
        if (monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {
            sunk = false;
            laserTime = 0;
        }

    }
    void setFlipOff(float flip)
    {
        if (flip == -1)
        {
            flipped = false;
        }
        if (flip == 1)
        {
            flipped = true;
        }
    }
   
private void OnTriggerEnter2D(Collider2D other) {
    if(other.tag =="Player"&&knockback){
         other.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,1));
    }
}

}
