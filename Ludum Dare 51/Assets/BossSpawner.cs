using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prawn;
    public GameObject burger;
    public GameObject Pizza;
    public Transform PrawnSpawnLocation;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void spawnPrawn()
    {
        Instantiate(prawn, PrawnSpawnLocation.position, Quaternion.identity);
    }
    public void spawnPizza()
    {
        Instantiate(Pizza, transform.position, Quaternion.identity);
    }
    public void spawnBurger()
    {
        Instantiate(burger, transform.position, Quaternion.identity);
    }
}
