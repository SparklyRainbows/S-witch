using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : EnemyBullet {
    private Vector3 targetPos;

    private void Start() {
        color = GameInformation.indestructibleColor;
        speed = .2f;
        damage = 1;
    }
    
    #region attack funcs
    public void StartMovingForward(float angle) {
        StartCoroutine(FingerMoveForward(angle));
    }

    public void StartMoving() {
        StartCoroutine(FingerMove());
    }

    private IEnumerator FingerMoveForward(float angle) {
        SetTarget(angle);
        
        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FingerMove() {
        SetTarget();

        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            yield return new WaitForEndOfFrame();
        }
    }

    public float GetPlayerToFingerAngle() {
        Vector3 dir = transform.position - GameObject.FindWithTag(GameInformation.playerTag).transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return angle + 90;
    }

    private void SetTarget(float angle) {
        targetPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) + transform.position;
        float slope = (targetPos.y - transform.position.y) / (targetPos.x - transform.position.x);
        if (targetPos.x > transform.position.x) {
            targetPos.x += 100;
        } else {
            targetPos.x -= 100;
        }
        targetPos.y = targetPos.x * slope;
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
    #endregion
}
