using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBullet : EnemyBullet
{
    private Vector3 targetPos;
    
    private float rotationSpeed;
    private float numOfRotations;

    public ParticleSystem pinkBurst;
    public ParticleSystem purpleBurst;

    private void Start() {
        speed = .2f;
        damage = 1;

        SetColor(Random.Range(0, 2) == 0 ? GameInformation.purple : GameInformation.pink);

        rotationSpeed = Random.Range(50, 80);
        numOfRotations = Random.Range(1, 5);

        StartCoroutine(Spin());
    }

    protected override void Move() {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
    }

    private IEnumerator Spin() {
        numOfRotations /= Time.deltaTime;
        while (numOfRotations > 0) {
            transform.Rotate(new Vector3(0, 0, 6f * rotationSpeed * Time.deltaTime));
            numOfRotations--;

            yield return new WaitForEndOfFrame();
        }

        SetTarget();
        //LookAtPlayer();
    }

    private void LookAtPlayer() {
        Vector3 dir = transform.position - targetPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

    public void SetTarget(float angle) {
        float distance = 5f;
        angle = angle / 180 * Mathf.PI;

        targetPos = transform.position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
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
