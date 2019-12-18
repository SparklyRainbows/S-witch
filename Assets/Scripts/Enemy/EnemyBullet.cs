using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public float damage;
    public float health;

    #region collision_functions
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (GameManager.instance.IsGameOver())
            return;

        if (collision.CompareTag(GameInformation.playerOneTag) || collision.CompareTag(GameInformation.playerTwoTag)) {
            OnPlayerHit();
        }
        
        if (collision.CompareTag(GameInformation.playerSpellTag) && collision.GetComponent<Bullet>().IsColor(color)) {
            health -= collision.gameObject.GetComponent<PlayerBullet>().damage;
            if (health <= 0)
            {
                OnHitBullet();
            }
        }
    }

    protected virtual void OnPlayerHit() {
        DestroySelf();
    }

    public void Destroy()
    {
        DestroySelf();
    }
    #endregion
}
