using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public bool burgerKing;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;
    public GameObject deathParticles;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public Rigidbody2D rb;
    private float dazedTime;
    public float startDazedTime;
    [SerializeField]
    private string idleAnimation;
    [SerializeField]
    private bool stationary;
    [SerializeField]
    private Color flashColor;
    [SerializeField]
    private float flashDuration;
    Material mat;
    [SerializeField]
    private Material[] mats;
    private IEnumerator flashCoroutine;
    public SpriteRenderer[] sprites;
    // Start is called before the first frame update
    private void Awake()
    {
        int count = 0;
        if (!burgerKing)
        {
            mat = GetComponent<SpriteRenderer>().material;
        }
        else
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer children in sprites)
                mats[count] = GetComponentInChildren<SpriteRenderer>().material;
            count++;
        }
    }
    public float getCurrentHealth(){
        return currentHealth;
    }
    void Start()
    {
        if (!burgerKing)
        {
            mat.SetColor("_FlashColor", flashColor);
        }
        else
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetColor("_FlashColor", flashColor);
            }
        }
        if(GetComponent<Animator>()!=null){
        animator = GetComponent<Animator>();
        }else{
            animator = GetComponentInParent<Animator>();
        }
        if(!bigPrawntoggle){
        rb = GetComponent<Rigidbody2D>();
        }else{
            rb = GetComponentInParent<Rigidbody2D>();
        }
        currentHealth = maxHealth;
    }
    public bool bigPrawntoggle;
    public bool Boss;

    // Update is called once per frame
    void Update()
    {
        if (dazedTime <= 0)
        {
            //return to normal
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            animator.Play(idleAnimation);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            dazedTime -= Time.deltaTime;
        }
        if (!stationary)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        checkGrounded();
        if (currentHealth <= 0)
            die();
    }
    void checkGrounded()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .15f, whatIsGround);
    }

    public bool getGrounded()
    {
        return isGrounded;
    }
    public void takeDamage(float damage)
    {
        flash();
        dazedTime = startDazedTime;
        currentHealth -= damage;
    }
    public GameObject bossDrop;
     
     public GameObject prawnself;
    public void die()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            
            if(Boss){
            Instantiate(bossDrop,FindObjectOfType<PlayerManager>().gameObject.transform.position+new Vector3(10,0,0),Quaternion.identity);
            }
            if(!bigPrawntoggle)
            Destroy(this.gameObject);
            if(bigPrawntoggle){
                prawnself = FindObjectOfType<BigPrawn>().gameObject;
                Destroy(prawnself);
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
        if (!burgerKing)
        {
            mat.SetFloat("_FlashAmount", flashAmount);
        }
        else
        {
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetFloat("_FlashAmount", flashAmount);
            }
        }
    }
}
