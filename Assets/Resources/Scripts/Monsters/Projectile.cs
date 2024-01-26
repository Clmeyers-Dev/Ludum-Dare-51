using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject impactParticles;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    public void setSpeed(float s)
    {
        speed = s;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //    
        if (other.tag == "Player" || other.tag =="Ground")
        {
            if( other.GetComponent<PlayerManager>()!=null)
            other.GetComponent<PlayerManager>().loseHealth();
            Instantiate(impactParticles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (other.tag == "enemy")
        {
            //something
        }
        if(other.tag =="Ground")
        {
            Instantiate(impactParticles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
