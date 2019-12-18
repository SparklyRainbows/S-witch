using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frankenstein : Enemy {
    float attackDelay = 3f;

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
        GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        bullet.GetComponent<Hand>().SetTarget(Random.Range(120, 250));
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
