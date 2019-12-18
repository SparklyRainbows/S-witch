using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy {
    float attackDelay = 1f;

    float colorSwitchTimer = 4f;
    //Color currentColor = GameInformation.purple;

    protected override void Start() {
        currentColor = GameInformation.purple;
        totalHealth = 100f;

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
