using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinBullet : EnemyBullet
{
    private Vector3 targetPos;
    public ParticleSystem pinkBurst;
    public ParticleSystem purpleBurst;

    private void Start() {
        SetTarget();
        speed = .1f;
    }

    private void SetTarget() {
        targetPos = GameObject.FindWithTag(GameInformation.playerTag).transform.position;
        float slope = (targetPos.y - transform.position.y) / (targetPos.x - transform.position.x);
        if (targetPos.x > transform.position.x) {
            targetPos.x += 100;
        } else {
            targetPos.x -= 100;
        }
        targetPos.y = targetPos.x * slope;
    }

    protected override void Move() {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
    }

    protected override void DestroySelf()
    {
        if (IsColor(GameInformation.pink))
        {
            Instantiate(pinkBurst, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(purpleBurst, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
