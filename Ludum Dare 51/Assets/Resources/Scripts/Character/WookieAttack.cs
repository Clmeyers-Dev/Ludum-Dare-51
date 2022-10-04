using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WookieAttack : MonoBehaviour
{
    private float timeBtwAttack;
    [SerializeField]
    private float startTimeBtwAttack;
    public Transform attackPos;
    [SerializeField]
    private float AttackRange;
    [SerializeField]
    private LayerMask mask;
    public int damage;
    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.Play("chomp");
                Controller controller = FindObjectOfType<Controller>();
                controller.audioManager.play("Chomp");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, AttackRange, mask);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<MonsterManager>().takeDamage(damage);
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position,AttackRange);
    }
}
