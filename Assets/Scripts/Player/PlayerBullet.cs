using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public float damage = 1;

    public ParticleSystem bigBurst;
    public ParticleSystem lilBurst;

    private Vector3 direction;

    private void Start() {
        speed = .2f;
    }

    public void SetDirection(Vector3 dir) {
        direction = dir;
        direction.Normalize();
    }

    protected override void Move() {
        transform.position += direction * speed;
    }

    #region collision_functions
    protected virtual void OnTriggerEnter2D(Collider2D collision) {

        if (GameManager.instance.IsGameOver())
            return;

        if (collision.CompareTag(GameInformation.enemyTag)) {

            createParticle(collision.GetComponent<Enemy>().GetCurrentEnemyColor());

            OnEnemyHit();
        }

        if (collision.CompareTag("Turret"))
        {
            if (collision.GetComponent<Turret>().GetColor() == GameInformation.nullColor)
            {
                if (IsColor(GameInformation.pink))
                {
                    ParticleSystem.MainModule main = lilBurst.main;
                    main.startColor = GameInformation.pink;
                }
                else
                {
                    ParticleSystem.MainModule main = lilBurst.main;
                    main.startColor = GameInformation.purple;
                }
                Instantiate(lilBurst, transform.position, Quaternion.identity);
            }
            else
            {
                createParticle(collision.GetComponent<Turret>().GetColor());
            }

            OnEnemyHit();
        }

        if (collision.CompareTag(GameInformation.enemyBulletTag)) {
            if (GameInformation.IsFinger(collision.name))
                return;

            createParticle(collision.GetComponent<Bullet>().GetColor());

            OnHitBullet();
        } 
    }

    protected virtual void OnEnemyHit() {
        DestroySelf();
    }
    #endregion

    #region Particles
    private void createParticle(Color enemyColor)
    {
        if (IsColor(enemyColor))
        {
            if (IsColor(GameInformation.pink))
            {
                ParticleSystem.MainModule main = bigBurst.main;
                main.startColor = GameInformation.pink;
            }
            else
            {
                ParticleSystem.MainModule main = bigBurst.main;
                main.startColor = GameInformation.purple;
            }
            Instantiate(bigBurst, transform.position, Quaternion.identity);
        }
        else
        {
            if (IsColor(GameInformation.pink))
            {
                ParticleSystem.MainModule main = lilBurst.main;
                main.startColor = GameInformation.pink;
            }
            else
            {
                ParticleSystem.MainModule main = lilBurst.main;
                main.startColor = GameInformation.purple;
            }
            Instantiate(lilBurst, transform.position, Quaternion.identity);
        }
    }
    #endregion
}
