using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShockwaveScript : Spell
{
    [SerializeField]
    [Tooltip("the shockwave")]
    private GameObject wave;

    [SerializeField]
    [Tooltip("the rate of expansion")]
    private float rate;

    [SerializeField]
    [Tooltip("the max radius")]
    private float maxRadius;

    void Start()
    {
        transform.localScale = new Vector3(.01f, .01f, .01f);
    }

    void Update()
    {
        transform.localScale += new Vector3(rate, rate, rate);
        if (transform.localScale.x > maxRadius)
        {
            Destroy(wave);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if  (col.gameObject.CompareTag("EnemyProjectile"))
        {
            col.gameObject.GetComponent<EnemyBullet>().Destroy();
        }
    }
}