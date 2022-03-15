using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    public LayerMask enemyLayers;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            Attack();
        }
        
        if (Input.GetKeyDown("x"))
        {
            DoubleAttack();
        }
    }

    void Attack() 
    {
        // Play animation
        animator.SetTrigger("Attack"); 
    
        // Detect Enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        // Apply damage
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Goblin>().TakeDamage(attackDamage);
            Debug.Log(enemy.name + " hit");
        }
    }

    void DoubleAttack()
    {
        animator.SetBool("Attacking", true);
        
        animator.SetTrigger("DoubleAttack");
        playerMovement.Stop();
        
    }

    void attakingFalse()
    {
        animator.SetBool("Attacking", false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
