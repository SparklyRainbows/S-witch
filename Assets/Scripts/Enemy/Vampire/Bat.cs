using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyBullet {
    private Vector3 targetPos;
    private Vector3 unscaledTargetPos;

    private float wobbleTimer = .1f;
    private float wobbleRange = 10f;

    public ParticleSystem batBurst;

    private void Start() {
        SetTarget();
        speed = .1f;
        damage = 1;
        color = GameInformation.indestructibleColor;

        StartCoroutine(Wobble());
    }

    private IEnumerator Wobble() {
        while (true) {
            yield return new WaitForSeconds(wobbleTimer);

            float currAngle = Mathf.Atan2(unscaledTargetPos.y - transform.position.y, unscaledTargetPos.x - transform.position.x) * 180 / Mathf.PI;
            float wobbleAngle = Random.Range(-wobbleRange, wobbleRange) + currAngle;
            unscaledTargetPos += new Vector3(Mathf.Cos(wobbleAngle), Mathf.Sin(wobbleAngle));

            ExtendTarget();
        }
    }

    private void ExtendTarget() {
        float slope = (unscaledTargetPos.y - transform.position.y) / (unscaledTargetPos.x - transform.position.x);
        if (unscaledTargetPos.x > transform.position.x) {
            targetPos.x += 100;
        } else {
            targetPos.x -= 100;
        }
        targetPos.y = targetPos.x * slope;
    }

    private void SetTarget() {
        targetPos = GameObject.FindWithTag(GameInformation.playerTag).transform.position;
        float slope = (targetPos.y - transform.position.y) / (targetPos.x - transform.position.x);
        unscaledTargetPos = targetPos;

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
        Instantiate(batBurst, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
