using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public AudioManager am;
    public Animator animator;
    public PlayerMovement playerMovement;
    public LayerMask enemyLayers;
    public LayerMask enemyRangedLayers;
    public HealthBar healthBar;

    public int maxHealth = 25;
    public int currentHealth;
    
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public Transform throwPointRight;
    public Transform throwPointLeft;

    public GameObject rockRightPrefab;
    public GameObject rockLeftPrefab;

    public float attackRangeRight = 0.5f;
    public float attackRangeLeft = 0.5f;
    public int attackDamage = 1;
    public float reloadTimer = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        FindObjectOfType<GameManager>().enemies = 4;
        am = FindObjectOfType<AudioManager>();
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

        if (Input.GetKeyDown("c"))
        {
            Throw();
        }
        if(reloadTimer <= 0f) reloadTimer = 1f; // wait 1 second before reloading
        
        //wait for reload
        if(reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer <= 0f)
            {
                enableCanAttack();
                animator.SetBool("Attacking", false);
            }
        }

        if (FindObjectOfType<GameManager>().enemies <= 0)
        {
            Invoke("NextLevel", 3f);
        }
    } 

    void Attack() 
    {
        if (animator.GetBool("CanAttack"))
        {
            am.Play("Punch");
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
            // hitEnemies.GetComponent<MeleeEnemy>().TakeDamage(attackDamage);
            int count = 0;
            // Apply damage
            foreach(Collider2D enemy in hitEnemies)
            {
                if (count < 1)
                {    
                    enemy.GetComponent<MeleeEnemy>().TakeDamage(attackDamage);
                    Debug.Log(enemy.name + " hit");
                }
                count++;
            }

            Collider2D[] hitRangedEnemies = Physics2D.OverlapCircleAll(position, range, enemyRangedLayers);
            // hitRangedEnemies.GetComponent<MeleeEnemy>().TakeDamage(attackDamage);
            count = 0;
            // Apply damage
            foreach(Collider2D enemy in hitRangedEnemies)
            {
                if (count < 1)
                {    
                    enemy.GetComponent<RangedEnemy>().TakeDamage(attackDamage);
                    Debug.Log(enemy.name + " hit");
                }
                count++;
            }
            
            
        }
        else 
        {
            // play sound
        }
        
    }

    void DoubleAttack()
    {
        if (animator.GetBool("CanAttack"))
        {
            am.Play2("Punch");
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
            // hitEnemies.GetComponent<MeleeEnemy>().TakeDamage(attackDamage);
            int count = 0;
            // Apply damage
            foreach(Collider2D enemy in hitEnemies)
            {
                if (count < 1)
                {    
                    enemy.GetComponent<MeleeEnemy>().TakeDamage(attackDamage*2);
                    Debug.Log(enemy.name + " hit");
                }
                count++;
            }

            Collider2D[] hitRangedEnemies = Physics2D.OverlapCircleAll(position, range, enemyRangedLayers);
            // hitRangedEnemies.GetComponent<MeleeEnemy>().TakeDamage(attackDamage);
            count = 0;
            // Apply damage
            foreach(Collider2D enemy in hitRangedEnemies)
            {
                if (count < 1)
                {    
                    enemy.GetComponent<RangedEnemy>().TakeDamage(attackDamage*2);
                    Debug.Log(enemy.name + " hit");
                }
                count++;
            }
            
            animator.SetBool("CanAttack", false);
        } else 
        {
            // play sound
        }
    }

    void Throw()
    {
        if (animator.GetBool("CanAttack"))
        {
            am.Play("Throw");
            animator.SetBool("CanAttack", false);
            animator.SetTrigger("Throw");
            playerMovement.Stop();

            if (animator.GetFloat("LastMove") == 1)
            {
                Instantiate(rockRightPrefab, throwPointRight.position, throwPointRight.rotation);
            } else {
                Instantiate(rockLeftPrefab, throwPointLeft.position, throwPointLeft.rotation);

            }
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
        am.Play("Ouch");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("(" + currentHealth + "/" + maxHealth);
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
            Invoke("GameOver", 2f);
        }
    }

    void Die()
    {
        animator.SetBool("CanAttack", false);
        animator.SetBool("isDead", true);
        playerMovement.Stop();
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public void selfDestroy()
    {
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    void GameOver()
    {
        FindObjectOfType<GameManager>().EndGame();
    }

    void NextLevel()
    {
        FindObjectOfType<GameManager>().NextLevel();
    }

}
