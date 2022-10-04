using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] monsters;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void foodFight(){
        if(Random.Range(0,2) ==0){
            spawnPizza();
        }
        if(Random.Range(0,2) ==0){
        spawnPrawn();
        }
        if(Random.Range(0,2) ==0){
        spawnBurger();
        }
    }
      public float pizzaOffset;
      public float burgerOffset;
    public void spawnPizza()
    {
        Instantiate(monsters[0],new Vector3(transform.position.x+ pizzaOffset, transform.position.y), Quaternion.identity);
    }
    public void spawnBurger()
    {
        Instantiate(monsters[1],new Vector3(transform.position.x , transform.position.y+burgerOffset), Quaternion.identity);
    }
    public float shrimpOffset;
    public void spawnPrawn()
    {
        Instantiate(monsters[2], new Vector3(transform.position.x, transform.position.y + shrimpOffset, transform.position.z), Quaternion.identity);
    }
     public void spawnFood()
    {
        Instantiate(monsters[3], new Vector3(transform.position.x, transform.position.y , transform.position.z), Quaternion.identity);
    }
}
