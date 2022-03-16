using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public Animator animator;

    public float attackTimer;

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
    }   

    public void selfDestroy()
    {
        Destroy(gameObject);
    }
}
