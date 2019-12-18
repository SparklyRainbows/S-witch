using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    protected float speed = 2f;

    private void Update() {
        Move();
    }

    protected virtual void Move() {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(GameInformation.playerOneTag) || collision.CompareTag(GameInformation.playerTwoTag)) {
            OnCollision(collision.gameObject.GetComponentInParent<UnitBehavior>());
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollision(UnitBehavior player) {
    }
}
