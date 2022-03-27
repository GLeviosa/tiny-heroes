using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellOrbLeft : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = -transform.right * speed;
    }

    void Update()
    {
        if(transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerCombat player = hitInfo.GetComponent<PlayerCombat>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);

        }
        

    }
}

