using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWinScreen : MonoBehaviour
{
    public Sprite pumpkin;
    public Sprite ghost;
    public Sprite skeleton;
    public Sprite frankenstein;
    public Sprite cat;
    public Sprite vampire;
    public Sprite scientist;

    private void Start() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Boss boss = GameManager.instance.GetCurrentBoss();
        Sprite bg = null;

        switch(boss) {
            case Boss.PUMPKIN:
                bg = pumpkin;
                break;
            case Boss.GHOST:
                bg = ghost;
                break;
            case Boss.SKELETON:
                bg = skeleton;
                break;
            case Boss.FRANKENSTEIN:
                bg = frankenstein;
                break;
            case Boss.CAT:
                bg = cat;
                break;
            case Boss.VAMPIRE:
                bg = vampire;
                break;
            case Boss.SCIENTIST:
                bg = scientist;
                break;
            default:
                Debug.LogWarning($"Boss not found: {boss}");
                break;
        }

        renderer.sprite = bg;
    }
}
