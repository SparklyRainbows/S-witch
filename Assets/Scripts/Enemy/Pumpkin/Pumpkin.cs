using System.Collections;
using UnityEngine;

public class Pumpkin : Enemy
{
    float bulletAttackDelay = .5f;
    float attackDelay = 2.5f;
    int bulletsPerCycle = 10;

    //Color currentColor = GameInformation.nullColor;

    protected override void Start() {
        currentColor = GameInformation.nullColor;
        totalHealth = 100f;
        deathClip = "pumpkin_die";

        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag)) {
            if (currentColor == GameInformation.nullColor ||
                collision.gameObject.GetComponent<Bullet>().IsColor(currentColor)) { 
                TakeDamage(collision.GetComponent<PlayerBullet>().damage);
            }
        }
    }

    #region attack_funcs

    protected override IEnumerator Attack() {
        while (true) {
            if (GameManager.instance.IsGameOver()) {
                break;
            }

            int bulletsFired = 0;

            Color bulletColor;

            int attackType = Random.Range(0, 2);
            string attack;
            if (attackType == 0) {
                bulletColor = GameInformation.purple;
                attack = "purple";
            } else {
                bulletColor = GameInformation.pink;
                attack = "pink";
            }

            currentColor = bulletColor;
            GameManager.instance.SetBossColor(currentColor);

            animator.SetBool(attack, true);

            while (bulletsFired < bulletsPerCycle) {
                if (GameManager.instance.IsGameOver()) {
                    break;
                }

                PlayShoot();
                bulletsFired++;
                yield return BulletEnum(bulletColor, ShootBullets);
            }

            animator.SetBool(attack, false);
            currentColor = GameInformation.nullColor;

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private IEnumerator BulletEnum(Color color, AttackFunc attack) {
        attack(color);
        yield return new WaitForSeconds(bulletAttackDelay);
    }

    private delegate void AttackFunc(Color color);

    private void ShootBullets(Color color) {
        GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().SetColor(color);
    }

    private void PurpleAndPinkAttack() {
        ShootBullets(GameInformation.purple);
        ShootBullets(GameInformation.pink);
    }
    #endregion

    protected override void Die() {
        animator.SetBool("pink", false);
        animator.SetBool("purple", false);

        base.Die();
    }
}
