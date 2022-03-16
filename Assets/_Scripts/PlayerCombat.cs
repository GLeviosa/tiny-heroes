using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    public LayerMask enemyLayers;

    public int maxHealth = 25;
    public int currentHealth;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRangeRight = 0.5f;
    public float attackRangeLeft = 0.5f;
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
        if (animator.GetBool("CanAttack"))
        {
            // Play animation
            animator.SetTrigger("Attack"); 
            animator.SetBool("CanAttack", false);

            Vector2 position;
            float range;
            // Check animation direction
            if (animator.GetFloat("LastMove") == 1)
            {
                position = attackPointRight.position;
                range = attackRangeRight;
            } else {
                position = attackPointLeft.position;
                range = attackRangeLeft;
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(position, range, enemyLayers);
            // Apply damage
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<MeleeEnemy>().TakeDamage(attackDamage);
                Debug.Log(enemy.name + " hit");
            }
            

        } else 
        {
            // play sound
        }
        
    }

    void DoubleAttack()
    {
        if (animator.GetBool("CanAttack"))
        {
            
            animator.SetBool("Attacking", true);
            animator.SetTrigger("DoubleAttack");
            playerMovement.Stop();
            
            Vector2 position = new Vector2();
            float range = 0;

            // Check animation direction
            if (animator.GetFloat("LastMove") == 1)
            {
                position = attackPointRight.position;
                range = attackRangeRight;
            }
            if (animator.GetFloat("LastMove") == -1){
                position = attackPointLeft.position;
                range = attackRangeLeft;
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(position, range, enemyLayers);
            // Apply damage
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<MeleeEnemy>().TakeDamage(attackDamage  * 2);
                Debug.Log(enemy.name + " hit");
            }
            
            animator.SetBool("CanAttack", false);
        } else 
        {
            // play sound
        }
    }

    void attakingFalse()
    {
        animator.SetBool("Attacking", false);
    }

    void enableCanAttack()
    {
        animator.SetBool("CanAttack", true);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPointRight == null)
            return;
        Gizmos.DrawWireSphere(attackPointRight.position, attackRangeRight);

        if (attackPointLeft == null)
            return;
        Gizmos.DrawWireSphere(attackPointLeft.position, attackRangeLeft);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log("(" + currentHealth + "/" + maxHealth);
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() {
        //
    }

}
