using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public Animator animator;
    public LayerMask playerLayer;

    public float attackTimer;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRangeRight = 0.5f;
    public float attackRangeLeft = 0.5f;
    public int attackDamage = 2;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //found target, what now ?
        if(attackTimer <= 0f) attackTimer = 5f; // wait 1 second before attacking
    
        //wait for attack
        if(attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            
            if(attackTimer <= 0f)
            {
                attack();
            }
        }
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


    void Die()
    {
        animator.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void attack()
    {
        animator.SetTrigger("Attack");
        attackTimer = 0f; // reset cooldown

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

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(position, range, playerLayer);
        // Apply damage
        foreach(Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
            Debug.Log(player.name + " hit");
        }
    }   

    public void selfDestroy()
    {
        Destroy(gameObject);
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
}
