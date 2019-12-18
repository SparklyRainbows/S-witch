using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : SpawnItem
{
    public ParticleSystem healEffect;
    private float healAmount = 1f;

    protected override void OnCollision(UnitBehavior player) {
        player.GainHealth(healAmount);
        Instantiate(healEffect, transform.position, Quaternion.identity);
    }
}
