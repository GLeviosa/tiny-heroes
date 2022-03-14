using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
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
        animator.SetTrigger("Attack");
        // Play animation
        // Detect Enemies in range
        // Apply damage
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
}
