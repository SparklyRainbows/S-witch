using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : EnemyBullet
{
    private Vector3 targetPos;
    private Vector3 startPos;

    public ParticleSystem pinkBurst;
    public ParticleSystem purpleBurst;

    private bool notHome;

    private void Start() {
        speed = .2f;
        damage = 1;

        notHome = true;

        SetColor(Random.Range(0, 2) == 0 ? GameInformation.purple : GameInformation.pink);

        startPos = transform.position;

        SetTarget();
        StartCoroutine(Spin());
        StartCoroutine(BoomerangMove());
    }

    private IEnumerator BoomerangMove() {
        while (transform.position.x > -6.5f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.3f);

        while(true) {
            if (transform.position == startPos) {
                notHome = false;
                DestroySelf();
            }

            transform.position = Vector3.MoveTowards(transform.position, startPos, speed);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Spin() {
        float rotationSpeed = 60f;
        while (true) {
            transform.Rotate(new Vector3(0, 0, 6f * rotationSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
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

    protected override void DestroySelf()
    {
        if(notHome)
        {
            if (IsColor(GameInformation.pink))
            {
                Instantiate(pinkBurst, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(purpleBurst, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }
}
