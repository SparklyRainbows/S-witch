using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hairball : EnemyBullet
{
    [SerializeField]
    [Tooltip("how fast the hairball travels")]
    private float ballSpeed;

    [SerializeField]
    [Tooltip("how fast the hairball rotates")]
    private float rotateSpeed;

    [SerializeField]
    [Tooltip("how long it takes the ball to reach full size")]
    private float growthTime;

    [SerializeField]
    [Tooltip("full size the ball reaches")]
    private Vector3 ballSize;

    [SerializeField]
    [Tooltip("particles for pink hairball explosion")]
    private ParticleSystem hairballPoofPink;

    [SerializeField]
    [Tooltip("particles for purple hairball explosion")]
    private ParticleSystem hairballPoofPurple;

    private void Start()
    {
        StartCoroutine(grow());
    }
    // Update is called once per frame
    protected override void Move()
    {
    
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        
        transform.Translate(Vector3.left * Time.deltaTime * ballSpeed, Space.World);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.IsGameOver())
            return;

        if (collision.CompareTag(GameInformation.playerSpellTag) && collision.GetComponent<Bullet>().IsColor(color))
        {
            health -= collision.gameObject.GetComponent<PlayerBullet>().damage;
            if (health <= 0)
            {
                OnHitBullet();
            }
        }
    }

    protected override void DestroySelf()
    {
        if (IsColor(GameInformation.pink))
        {
            Instantiate(hairballPoofPink, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(hairballPoofPurple, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private IEnumerator grow() {

        float elapsedTime = 0.0f;
        while (elapsedTime < growthTime)
        {
            elapsedTime += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, ballSize, elapsedTime / growthTime);
            yield return null;
        }
    }

}
