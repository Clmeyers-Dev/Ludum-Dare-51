using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour
{
    [SerializeField]
    private float fireangle;
    [SerializeField]
    public float fireSpeed = 0.5f;
    [SerializeField]
    private float timer;
    public Transform firepoint;
    [SerializeField]
    private GameObject bullet;
    public GameObject parent;
    private MonsterManager monsterManager;
    public AudioSource audioPlayer;
    public AudioClip sound;
    [SerializeField]
    private int radius;
    public float currentSpeed;
    [SerializeField]
    private Transform Player;
    public void setBullet(GameObject b)
    {
        bullet = b;
    }
    public void setFireSpeed(float s)
    {
        currentSpeed = s;
    }
    void Start()
    {
        monsterManager = GetComponent<MonsterManager>();
        currentSpeed = fireSpeed;
        Player = FindObjectOfType<PlayerManager>().transform;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) <= radius && monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0))
        {
            monsterManager.animator.Play("attack");
        }
        if(Vector3.Distance(transform.position, Player.position) > radius && monsterManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !monsterManager.animator.IsInTransition(0)){
            monsterManager.animator.Play("idle");
        }
    }
    public void shoot()
    {
        var shot = Instantiate(bullet, firepoint.position, Quaternion.Euler(parent.transform.localEulerAngles.x, parent.transform.localEulerAngles.y, parent.transform.localEulerAngles.z + fireangle));
        //        audioPlayer.PlayOneShot(sound);
        //shot.GetComponent<Projectile>().setSpeed( player.getSpeed() + 20);
        timer = currentSpeed;

    }
     void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
