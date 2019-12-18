using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : EnemyBullet
{
    public AudioClip shoot;

    private List<GameObject> fingers;

    private float initialDelay = 4f;
    private float fireDelay = .5f;

    private Vector3 targetPos;

    private int health;
    private bool takingDamage;
    private SpriteRenderer renderer;
    private AudioSource audio;

    private void Start() {
        audio = GetComponent<AudioSource>();

        renderer = GetComponent<SpriteRenderer>();
        damage = 0;
        health = 5;
        speed = .3f;

        SetFingers();

        StartCoroutine(FireFingers());
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        LookAtPlayer();
    }

    #region damaged funcs
    protected override void OnHitBullet() {
        health -= 1;
        if (health <= 0) {
            DestroySelf();
        }

        if (!takingDamage) {
            StartCoroutine(TakeDamage());
        }
    }

    protected override void DestroySelf() {
        StartCoroutine(FireAllFingers());
    }

    protected IEnumerator TakeDamage() {
        takingDamage = true;
        Color prevColor = renderer.color;

        float flashDelay = .1f;
        for (int i = 0; i < 3; i++) {
            renderer.color = Color.red;
            yield return new WaitForSeconds(flashDelay);

            renderer.color = prevColor;
            yield return new WaitForSeconds(flashDelay);
        }

        takingDamage = false;
    }
    #endregion

    #region move funcs
    protected override void Move() {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
    }

    public void SetTarget(float angle) {
        float distance = 5f;
        angle = angle / 180 * Mathf.PI;

        targetPos = transform.position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
    }
    #endregion

    #region attack funcs
    private IEnumerator FireAllFingers() {
        if (fingers.Count == 0) {
            base.DestroySelf();
            yield break;
        }

        float angle = 0;
        float angleChange = 0;
        try {
            angleChange = 360 / fingers.Count;
        } catch(Exception e) {
            base.DestroySelf();
            yield break;
        }

        PlayShoot();
        while (fingers.Count > 0) {
            FireFinger(angle);
            angle += angleChange;
            yield return null;
        }

        base.DestroySelf();
    }

    private IEnumerator FireFingers() {
        yield return new WaitForSeconds(initialDelay);

        while (fingers.Count > 0) {
            yield return new WaitForSeconds(fireDelay);
            PlayShoot();
            FireFinger();
        }

        base.DestroySelf();
    }

    private void LookAtPlayer() {
        if (fingers.Count <= 0) {
            return;
        }

        float angle = fingers[0].GetComponent<Finger>().GetPlayerToFingerAngle();

        if (fingers[0].name.Equals("Thumb")) {
            Vector3 dir = transform.position - GameObject.FindWithTag(GameInformation.playerTag).transform.position;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void FireFinger(float angle) {
        if (fingers.Count == 0)
            return;

        GameObject fingerObj = fingers[0];
        fingers.RemoveAt(0);
        fingerObj.transform.SetParent(null);
        fingerObj.transform.position = transform.position;

        fingerObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        fingerObj.GetComponent<Finger>().StartMovingForward(angle);
    }

    private void FireFinger() {
        GameObject fingerObj = fingers[0];
        fingers.RemoveAt(0);
        fingerObj.transform.SetParent(null);

        fingerObj.GetComponent<Finger>().StartMoving();
    }

    private void SetFingers() {
        fingers = new List<GameObject>();
        foreach (Transform child in transform) {
            fingers.Add(child.gameObject);
        }
    }
    #endregion

    private void PlayShoot() {
        audio.clip = shoot;
        audio.Play();
    }
}
