using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prawn;

    [SerializeField]
    private GameObject burger;

    [SerializeField]
    private GameObject pizza;

    [SerializeField]
    private Transform prawnSpawnLocation;

    public void SpawnPrawn()
    {
        Instantiate(prawn, prawnSpawnLocation.position, Quaternion.identity);
    }

    public void SpawnPizza()
    {
        Instantiate(pizza, transform.position, Quaternion.identity);
    }

    public void SpawnBurger()
    {
        Instantiate(burger, transform.position, Quaternion.identity);
    }
}
