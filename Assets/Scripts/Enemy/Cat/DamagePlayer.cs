using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The damage this boss deals of you make contact with it")]
    float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameInformation.playerOneTag) || collision.gameObject.CompareTag(GameInformation.playerTwoTag))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
