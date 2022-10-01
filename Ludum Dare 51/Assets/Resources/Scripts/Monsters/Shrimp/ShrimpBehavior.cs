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
   Vector3 start;
    void Start()
    {
        start = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeBtwHits < maxBtwHits)
        {
            timeBtwHits += Time.deltaTime;
            fly();
        }
        else
        {
            canHit = true;
            swoop();
        }
        fly();
    }
    void fly()
    {
        transform.position = start+Vector3.left*Mathf.Sin(Time.realtimeSinceStartup)*offset;
    }
    void swoop()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && canHit)
        {
            canHit = false;
            PlayerManager pm = other.GetComponent<PlayerManager>();
            pm.loseHealth();
            timeBtwHits = 0;
        }

    }
}
