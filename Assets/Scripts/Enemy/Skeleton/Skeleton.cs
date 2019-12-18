using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public GameObject boomerangBullet;

    float attackDelay = .9f;

    float colorSwitchTimer = 4f;
    //Color currentColor = GameInformation.purple;

    protected override void Start() {
        totalHealth = 100f;
        currentColor = GameInformation.purple;

        StartCoroutine(SwitchColor());
        base.Start();
    }

    private IEnumerator SwitchColor() {
        while (true) {
            currentColor = currentColor == GameInformation.purple ? GameInformation.pink : GameInformation.purple;
            GetComponent<SpriteRenderer>().color = currentColor;
            GameManager.instance.SetBossColor(currentColor);

            yield return new WaitForSeconds(colorSwitchTimer);
        }
    }

    #region attack_funcs
    protected override IEnumerator Attack() {
        while (true) {
            if (GameManager.instance.IsGameOver()) {
                break;
            }

            PlayShoot();
            ShootBullet();
            yield return new WaitForSeconds(attackDelay);
        }
    }

    private void ShootBullet() {
        float random = Random.Range(0, 2);
        if (random == 0) {
            ShootBoomerangBullet();
        } else {
            ShootSpinningBullet();
        }
    }

    private void ShootSpinningBullet() {
        GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        bullet.GetComponent<SpinningBullet>().SetTarget(Random.Range(120, 250));
    }

    private void ShootBoomerangBullet() {
        GameObject bullet = Instantiate(boomerangBullet, transform.position, Quaternion.identity);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag)) {
            if (currentColor == GameInformation.nullColor ||
                collision.gameObject.GetComponent<Bullet>().IsColor(currentColor)) {
                TakeDamage(collision.GetComponent<PlayerBullet>().damage);
            }
        }
    }
}
