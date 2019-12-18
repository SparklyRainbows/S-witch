using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHat : MonoBehaviour
{
    private Color currentColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = GameInformation.nullColor;
        currentColor = spriteRenderer.color;
    }

    public void setSpriteColor(Color color)
    {
        spriteRenderer.color = color;

    }

    public void setColor(Color color)
    {
        currentColor = color;
    }

    public Color getColor()
    {
        return currentColor;
    }
}
