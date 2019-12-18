using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Color color = GameInformation.nullColor;
    protected float speed;

    protected virtual void FixedUpdate() {
        Move();
        DestroyOffscreen();
    }

    protected virtual void OnHitBullet() {
        DestroySelf();
    }

    protected virtual void DestroySelf() {
        Destroy(gameObject);
    }

    #region movement_functions
    protected virtual void Move() {
        
    }

    private void DestroyOffscreen() {
        float offset = 50;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height + offset || screenPosition.y < -offset ||
            screenPosition.x > Screen.width + offset || screenPosition.x < -offset) {
            Destroy(gameObject);
        }
    }
    #endregion

    #region color_functions
    public void SetColor(Color c) {
        color = c;
        GetComponent<SpriteRenderer>().color = color;
    }

    public Color GetColor()
    {
        return color;
    }

    public bool IsColor(Color c) {
        return color == c || c == GameInformation.nullColor;
    }
    #endregion
}
