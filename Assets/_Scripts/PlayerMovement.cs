using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1) 
        {
            animator.SetFloat("LastMove", Input.GetAxisRaw("Horizontal"));
        } 

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) 
        {
            // Debug.Log(animator.GetFloat("LastMove"));
            animator.SetFloat("Horizontal", animator.GetFloat("LastMove"));
        
        } 
        

        // Debug.Log(Input.GetAxisRaw("Horizontal"));
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
