using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    private Color flashColor;
    [SerializeField]
    private float flashDuration;
    Material mat;
    private IEnumerator flashCoroutine;
    [SerializeField]
    private GameObject[] heartSprites = new GameObject[5];
    [SerializeField]
    private int hearts;
    [SerializeField]
    private int maxHearts;
    [SerializeField]
    private int currentHearts;
    // Start is called before the first frame update
    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }
    void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
        currentHearts = maxHearts;
    }

    // Update is called once per frame
    void Update()
    {

    }
    [SerializeField]
    private GameObject deathspawn;
    public void die()
    {
        var shot = Instantiate(deathspawn, transform.position, Quaternion.identity);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        TenSecondManager ten = FindObjectOfType<TenSecondManager>();
        ten.endScreen();


    }
    public void loseHealth()
    {
        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (heartSprites[i].activeSelf)
            {
                heartSprites[i].SetActive(false);
                flash();
                return;
            }
        }
    }
    void flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }
    public void gainHealth()
    {
        for (int i = heartSprites.Length - 1; i >= 0; i--)
        {
            if (!heartSprites[i].activeSelf)
            {
                heartSprites[i].SetActive(true);
                return;
            }
        }
    }
    public int getNumberOfHealth()
    {
        int count = 0;
        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (heartSprites[i].activeSelf)
            {
                count++;
            }
        }

        return count;
    }
    private IEnumerator DoFlash()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }

    private void SetFlashAmount(float flashAmount)
    {
        mat.SetFloat("_FlashAmount", flashAmount);
    }
}
