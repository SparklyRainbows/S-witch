using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : EnemyBullet
{
    [SerializeField]
    [Tooltip("how fast the beam travels")]
    private float laserSpeed;

    public ParticleSystem bulletBurst;

    private Vector3 velocity;

    private void Start()
    {
        velocity = Vector3.left;
    }

    protected override void Move()
    {
        transform.Translate(velocity * Time.deltaTime * laserSpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            velocity.x *= -1;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("HorizontalWall"))
        {
            velocity.y *= -1;
            Destroy(gameObject);
        }

        base.OnTriggerEnter2D(collision);
    }

    protected override void DestroySelf()
    {
        Instantiate(bulletBurst, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
