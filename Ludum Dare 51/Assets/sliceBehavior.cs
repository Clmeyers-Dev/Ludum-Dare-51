using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliceBehavior : MonoBehaviour
{
    // Start is called before the first frame update
      public Transform firepoint;
    [SerializeField]
    private GameObject bullet;
    public GameObject parent;
        [SerializeField]
    private float fireangle;
    [SerializeField]
    public float fireSpeed = 0.5f;
    public Sprite topping;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void shoot(){
                var shot = Instantiate(bullet, firepoint.position, Quaternion.Euler(parent.transform.localEulerAngles.x, parent.transform.localEulerAngles.y, parent.transform.localEulerAngles.z + fireangle));
                    shot.GetComponent<SpriteRenderer>().sprite = topping;
    }
}
