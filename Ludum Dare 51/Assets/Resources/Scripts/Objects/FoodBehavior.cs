using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    [SerializeField]
    private float healAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.tag =="Player"){
           other.transform.GetComponent<PlayerManager>().getFood(healAmount);
           Destroy(this.gameObject);
        }
    }
}
