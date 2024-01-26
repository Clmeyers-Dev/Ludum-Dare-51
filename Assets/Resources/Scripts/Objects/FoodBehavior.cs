using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    [SerializeField]
    private int healAmount;
    [SerializeField]
    private Sprite[] foodChoices;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = foodChoices[Random.Range(0,foodChoices.Length)];
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.tag =="Player"){
           other.transform.GetComponent<PlayerManager>().gainHealth();
           FindObjectOfType<TenSecondManager>().foodCount++;
           Destroy(this.gameObject);
        }
    }
}
