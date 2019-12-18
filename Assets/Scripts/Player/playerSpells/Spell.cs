using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    [Tooltip("how much mp the spell costs")]
    private float mpCost;

    [SerializeField]
    [Tooltip("sprite to set in UI")]
    private Sprite uiSprite;

    public float MPCost()
    {
        return mpCost;
    }
    public Sprite GetSprite()
    {
        return uiSprite;
    }
}
