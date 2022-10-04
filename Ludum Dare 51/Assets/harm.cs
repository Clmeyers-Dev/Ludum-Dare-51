using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harm : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeBtwHits;
    [SerializeField]
    private float maxBtwHits;
    public bool canHit = true;
    public bool knockBack = false;
    public float x;
    public float y;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwHits < maxBtwHits)
        {
            timeBtwHits += Time.deltaTime;
        }
        else
        {
            canHit = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && canHit)
        {
            canHit = false;
            PlayerManager pm = other.GetComponent<PlayerManager>();
            pm.loseHealth();
            timeBtwHits = 0;
            if (knockBack)
            {
                other.GetComponent<Rigidbody2D>().MovePosition(other.GetComponent<Rigidbody2D>().position +Vector2.left);
            }
        }

    }
}
