using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public AudioManager am;
    public Transform orbPointRight;
    public Transform orbPointLeft;
    public GameObject orbRightPrefab;
    public GameObject orbLeftPrefab;
    public int maxHealth = 10;
    public HealthBar healthBar;
    public int currentHealth;
    public Animator animator;
    public LayerMask playerLayer;
    public Transform NPC;
    public Transform Player;
    public Transform center;

    public float attackTimer = 1f;
    public int attackDamage = 2;

    public float speed = 1.0f;
    public float stoppingDistance = 5.0f;
    public float SpaceBetween = 1.5f;
    GameObject[] Other_Enemies;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Other_Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("dist y: " + Mathf.Abs(Player.position.y - transform.position.y));
        if (Vector2.Distance(NPC.position, Player.position) > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
            var heading = Player.position - NPC.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            // Debug.Log("Speed: " + direction.sqrMagnitude);
            // Debug.Log("X DIR: " + direction.x);
            animator.SetFloat("Speed", 1);
            avoidEnemies();
            if (direction.x > 0) {
                animator.SetFloat("LastMove", 1);
            } else {
                animator.SetFloat("LastMove", -1);
            }
            // Debug.Log("Vector: " + direction);

        } else if (Mathf.Abs(Player.position.y - transform.position.y) > 1){
            Vector3 newPos = transform.position;
            newPos.y = Mathf.MoveTowards(newPos.y, Player.position.y, speed * Time.deltaTime);
            transform.position = newPos;

            if (Player.position.x > transform.position.x) {
                animator.SetFloat("LastMove", 1);
            } else {
                animator.SetFloat("LastMove", -1);
            }

        } else {
            // Debug.Log("PLAYER FOUND!!!! : " + attackTimer);
            animator.SetFloat("Speed", 0);
            //found target, what now ?
            if(attackTimer <= 0f) attackTimer = 2f; // wait 1 second before attacking
        
            //wait for attack
            if(attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
                Debug.Log(attackTimer);
                if(attackTimer <= 0f)
                {
                    attack();
                }
            }
        }
        // float distCovered = (Time.time - startTime) * speed;
        // journeyLength = Vector2.Distance(NPC.position, Player.position);
        // float fractionOfJourney = distCovered / journeyLength;


        // transform.position = Vector2.Lerp(NPC.position, Player.position, fractionOfJourney*Time.fixedDeltaTime);
        // Debug.Log("MAGNITUDE: " + Vector2.Lerp(NPC.position, Player.position, fractionOfJourney).sqrMagnitude);

        
        
    }

    public void TakeDamage(int damage) {
        if (!animator.GetBool("isDead"))
        {
            am.Play("OuchMonster");
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            Debug.Log("(" + currentHealth + "/" + maxHealth);
            animator.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void avoidEnemies(){
        foreach (GameObject obj in Other_Enemies)
        {
            if (obj != gameObject && obj != null)
            {
                float distance_to_enemy = Vector3.Distance(obj.transform.position, center.position);
                
                if (distance_to_enemy <= SpaceBetween)
                {
                    Vector3 direction = center.position - obj.transform.position;
                    transform.Translate(direction * Time.deltaTime * speed);
                }
            }
        }
    }


    void Die()
    {
        animator.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        FindObjectOfType<GameManager>().enemies--;
    }

    void attack()
    {
        am.Play("Spell");
        animator.SetTrigger("Attack");
        attackTimer = 0f; // reset cooldown

        // Check animation direction
        if (animator.GetFloat("LastMove") == 1)
        {
            Instantiate(orbRightPrefab, orbPointRight.position, orbPointRight.rotation);
        } else {
            Instantiate(orbLeftPrefab, orbPointLeft.position, orbPointLeft.rotation);
        }

    }   

    public void selfDestroy()
    {
        Destroy(gameObject);
    }

}
