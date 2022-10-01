using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointTowardsPlayer : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private float turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerManager>().transform;
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 dir = 
         target.transform.position - this.transform.position;
     Quaternion rot = 
         Quaternion.LookRotation(Vector3.forward, dir);
     this.transform.rotation = 
         Quaternion.Lerp(this.transform.rotation, rot, Time.deltaTime * turnSpeed); 
    }
}
