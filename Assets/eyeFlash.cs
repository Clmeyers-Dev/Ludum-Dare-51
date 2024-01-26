using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeFlash : MonoBehaviour
{

    [SerializeField]
    private Color flashColor;
    [SerializeField]
    private float flashDuration;
    Material mat;
    private IEnumerator flashCoroutine;
    public GameObject thingToFlash;

    private void Awake()
    {
        mat =thingToFlash.GetComponent<SpriteRenderer>().material;
    }
    // Start is called before the first frame update
    void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
    }

    // Update is called once per frame
    void Update()
    {

    }
 public   void flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }
    private void SetFlashAmount(float flashAmount)
    {

        mat.SetFloat("_FlashAmount", flashAmount);

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
}
