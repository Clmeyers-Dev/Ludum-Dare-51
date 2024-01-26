using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration;
    private Material mat;
    private IEnumerator flashCoroutine;
    
    [SerializeField] private GameObject[] heartSprites = new GameObject[5];
    [SerializeField] private int maxHearts;
    private int currentHearts;

    [SerializeField] private GameObject deathspawn;

    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
        currentHearts = maxHearts;
        Flash();
    }


    public void Die()
    {
        Instantiate(deathspawn, transform.position, Quaternion.identity);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        TenSecondManager ten = FindObjectOfType<TenSecondManager>();
        ten.EndScreen();
    }

    public void LoseHealth()
    {
        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (heartSprites[i].activeSelf)
            {
                heartSprites[i].SetActive(false);
                Flash();
                return;
            }
        }
    }

    void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

    public void GainHealth()
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

    public int GetNumberOfHealth()
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
