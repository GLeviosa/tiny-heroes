using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
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
    }

    void Attack() 
    {
        animator.SetTrigger("Attack");
        // Play animation
        // Detect Enemies in range
        // Apply damage
    }
}
