using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDrop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public string objectName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < 4; i++)
            {
                FindObjectOfType<PlayerManager>().gainHealth();
            }

            switch (objectName)
            {
                case "Prawn":
                    FindObjectOfType<TenSecondManager>().PrawnCount++;
                    break;
                case "Burger":
                    FindObjectOfType<TenSecondManager>().BurgerCount++;
                    break;
                case "Pizza":
                    FindObjectOfType<TenSecondManager>().PizzaCount++;
                    break;
                default:
                    break;
            }

            FindObjectOfType<TenSecondManager>().unPause();
            Destroy(this.gameObject);
        }
    }
}
