using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrumwaldaGhost : Ghost
{
    protected override void Start() {
        theUnit = GameObject.FindGameObjectWithTag(GameInformation.playerTag);

        isAngry = true;
        ghostTransform = transform;
        ghostRB = GetComponent<Rigidbody2D>();
        unitTransform = theUnit.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        currentColor = GameInformation.nullColor;
        deathClip = "ghost_split";
        clipBool = "split";
        GetComponent<Collider2D>().enabled = true;
        StartCoroutine(Attack());

        GameManager.instance.SetBossColor(currentColor);
    }

    public override void TakeDamage(float damage) {
        if (GameManager.instance.IsGameOver()) {
            return;
        }

        ghostHealth -= damage;

        if (!takingDamage) {
            StartCoroutine(TakeDamage());
        }

        if (ghostHealth <= 0) {
            Die();
        }
    }

    protected override IEnumerator DeathAnimation() {
        yield return null;
        GameObject.Find("Grumwalda").GetComponent<Grumwalda>().KilledGhost();
        Destroy(gameObject);
    }
}
