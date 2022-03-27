using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockRight : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        MeleeEnemy enemy = hitInfo.GetComponent<MeleeEnemy>();
        RangedEnemy enemyRanged = hitInfo.GetComponent<RangedEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(1);
            Destroy(gameObject);

        }

        if (enemyRanged != null)
        {
            enemyRanged.TakeDamage(1);
            Destroy(gameObject);

        }
    }
}
