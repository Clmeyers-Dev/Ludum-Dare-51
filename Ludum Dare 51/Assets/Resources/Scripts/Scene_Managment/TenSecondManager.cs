using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TenSecondManager : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    public string[] states = new string[10];
    public string state;
    public bool paused;
    public Animator animator;
    public Vector3 returnLocation;
    public Transform BossRoomSpawn;
    public Transform PrawnRamSpan;
    public Transform player;
    public int foodCount;
    public int BurgerCount;
    public int PrawnCount;
    public int PizzaCount;
    public BossSpawner bossSpawner;
    private void Update()
    {
        if (!paused)
        {
            animator.Play("Spin");
        }
        else
        {
            animator.Play("Idle");
        }
        stateText.text = state;


    }

    public void unPause()
    {
        player.transform.position = returnLocation;
        paused = false;
    }
    void changeState()
    {
        int random = Random.Range(0, 101);
        Debug.Log(random);
        if (random >= 0 && random <= 10)
        {
            state = states[0];
            paused = true;
            //burger
        }
        else if (random > 10 && random <= 15)
        {
            state = states[1];
            paused = true;
            //prawn
        }
        else if (random > 15 && random <= 30)
        {
            state = states[2];
            paused = true;
            //pizza
        }
        else if (random > 30 && random <= 70)
        {
             paused = true;
            state = states[3];
        }
        else if (random > 70 && random <= 100)
        {
             paused = true;
            state = states[4];
        }
        if (state == "Big Prawn")
        {
            returnLocation = player.transform.position;
            player.transform.position = PrawnRamSpan.transform.position;
            bossSpawner.spawnPrawn();
        }
        if (state == "Burger King")
        {
            returnLocation = player.transform.position;
            player.transform.position = BossRoomSpawn.transform.position;
            bossSpawner.spawnBurger();
        }
        if (state == "Pinwheel Pizza")
        {
            returnLocation = player.transform.position;
            player.transform.position = BossRoomSpawn.transform.position;
            bossSpawner.spawnPizza();
        }
        if (state == "Fleas")
        {

        }
        if (state == "JUMP!!!")
        {

        }
    }
}
