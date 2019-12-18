using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBat : Enemy
{
    protected override void Start() {
        totalHealth = 5;
        currentHealth = totalHealth;

        renderer = GetComponent<SpriteRenderer>();
    }

    public override void TakeDamage(float damage) {
        currentHealth -= damage;

        if (!takingDamage) {
            StartCoroutine(TakeDamage());
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    protected override void Die() {
        Destroy(gameObject);
    }
}
