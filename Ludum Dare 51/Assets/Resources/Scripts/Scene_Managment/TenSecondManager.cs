using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
public class TenSecondManager : MonoBehaviour
{
    public GameObject fog;
    public TextMeshProUGUI pointTotal;
    public GameObject BossCamera;
    public GameObject normalCamera;
    public TextMeshProUGUI stateText;
    public float points;
    public float regularCameraSize;
    public float BossCameraSize;
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
    public GameObject prawnBackground;
    public GameObject burgerBackground;
    public GameObject pizzaBackground;
    public TextMeshProUGUI burgerCounttxt;
    public TextMeshProUGUI PrawnCounttxt;
    public TextMeshProUGUI PizzaCounttxt;
    public TextMeshProUGUI FoodCounttxt;
    public TextMeshProUGUI pointTotatl;
    public AudioSource bossMusic;
    public AudioSource normalMusic;
    private void Update()
    {

        points += Time.deltaTime;
        if (!paused)
        {
            animator.Play("Spin");
            normalCamera.SetActive(true);
            BossCamera.SetActive(false);
            normalMusic.Pause();

            bossMusic.UnPause();
            if (!bossMusic.isPlaying)
            {
                bossMusic.Play();
            }
        }
        else
        {
            animator.Play("Idle");

            normalMusic.UnPause();
            bossMusic.Pause();
            if (!normalMusic.isPlaying)
            {
                normalMusic.Play();
            }

        }
        stateText.text = state;


    }
    public GameObject endscreenPanel;
    public void endScreen()
    {
        points += foodCount * 10;
        points += BurgerCount * 50;
        points += PizzaCount * 50;
        points += PrawnCount * 100;
        burgerCounttxt.text = BurgerCount + "";
        PrawnCounttxt.text = PrawnCount + "";
        PizzaCounttxt.text = PizzaCount + "";
        FoodCounttxt.text = foodCount + "";
        pointTotatl.text = (int)points + "";


        endscreenPanel.SetActive(true);
    }
    public void restart()
    {
        GameObject wookie = FindObjectOfType<PlayerManager>().gameObject;
        Destroy(wookie);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        if (random >= 0 && random <= 18)
        {
            state = states[0];
            //steamed Hams
        }
        else if (random > 18 && random <= 33)
        {
            state = states[1];
            //food fight
        }
        else if (random > 33 && random <= 47)
        {
            state = states[2];
            //shrimp?
        }
        else if (random > 47 && random <= 62)
        {

            state = states[3];
            //Pizza Party
        }
        else if (random > 62 && random <= 76)
        {

            state = states[4];
            //free food
        }
        else if (random > 76 && random <= 89)
        {
            state = states[5];
            paused = true;
            normalCamera.SetActive(false);
            BossCamera.SetActive(true);
            //burger king
        }
        else if (random > 89 && random <= 95)
        {
            state = states[6];
            paused = true;
            normalCamera.SetActive(false);
            BossCamera.SetActive(true);
            //pinwheel
        }
        else if (random > 95 && random <= 101)
        {
            state = states[7];
            paused = true;
            normalCamera.SetActive(false);
            BossCamera.SetActive(true);
            //prawn
        }






        if (state == "Big Prawn")
        {
            prawnBackground.SetActive(true);
            burgerBackground.SetActive(false);
            pizzaBackground.SetActive(false);
            returnLocation = player.transform.position;
            player.transform.position = PrawnRamSpan.transform.position;


            bossSpawner.spawnPrawn();
        }
        if (state == "Burger King")
        {
            prawnBackground.SetActive(false);
            burgerBackground.SetActive(true);
            pizzaBackground.SetActive(false);
            returnLocation = player.transform.position;
            player.transform.position = BossRoomSpawn.transform.position;


            bossSpawner.spawnBurger();
        }
        if (state == "Pinwheel Pizza")
        {
            prawnBackground.SetActive(false);
            burgerBackground.SetActive(false);
            pizzaBackground.SetActive(true);
            returnLocation = player.transform.position;
            player.transform.position = BossRoomSpawn.transform.position;


            bossSpawner.spawnPizza();
        }
        if (state == "Pizza Party")
        {
            MonsterSpawn[] monsterSpawners;
            monsterSpawners = FindObjectsOfType<MonsterSpawn>();
            foreach (MonsterSpawn spawners in monsterSpawners)
            {
                spawners.spawnPizza();
            }
        }
        if (state == "Shrimp?")
        {
            MonsterSpawn[] monsterSpawners;
            monsterSpawners = FindObjectsOfType<MonsterSpawn>();
            foreach (MonsterSpawn spawners in monsterSpawners)
            {
                spawners.spawnPrawn();
            }
        }
        if (state == "Steamed Hams")
        {
            MonsterSpawn[] monsterSpawners;
            monsterSpawners = FindObjectsOfType<MonsterSpawn>();
            foreach (MonsterSpawn spawners in monsterSpawners)
            {
                spawners.spawnBurger();
            }
        }
        if (state == "Free Food")
        {
             MonsterSpawn[] monsterSpawners;
            monsterSpawners = FindObjectsOfType<MonsterSpawn>();
            foreach (MonsterSpawn spawners in monsterSpawners)
            {
                spawners.spawnFood();
            }
        }
        if (state == "Food Fight")
        {
            MonsterSpawn[] monsterSpawners;
            monsterSpawners = FindObjectsOfType<MonsterSpawn>();
            foreach (MonsterSpawn spawners in monsterSpawners)
            {
                spawners.foodFight();
            }
        }
    }
}
