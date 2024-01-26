using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WookieAttack : MonoBehaviour
{
    [SerializeField] private float startTimeBtwAttack;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float AttackRange;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int damage;
    [SerializeField] private Animator animator;

    private float timeBtwAttack;

    void Update()
    {
        if (timeBtwAttack <= 0 && Input.GetMouseButtonDown(0))
        {
            PerformAttack();
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, AttackRange);
    }

    void PerformAttack()
    {
        animator.Play("chomp");
        Controller controller = FindObjectOfType<Controller>();
        if (controller != null)
        {
            controller.audioManager.play("Chomp");
        }

        Collider2D[] enemies = new Collider2D[10]; // Adjust the size based on your needs
        int enemyCount = Physics2D.OverlapCircleNonAlloc(attackPos.position, AttackRange, enemies, mask);

        for (int i = 0; i < enemyCount; i++)
        {
            MonsterManager monsterManager = enemies[i].GetComponent<MonsterManager>();
            if (monsterManager != null)
            {
                monsterManager.takeDamage(damage);
            }
        }
    }
}
