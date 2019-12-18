using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectSpace : MonoBehaviour {
    public string witchColor;

    public Text text;
    public Image image;

    public void Connect() {
        text.text = $"{witchColor} witch connected! \nPress A to confirm.";
        SetTransparent(false);
    }

    public void Disconnect() {
        text.text = $"Press {GetInputType()} to be the {witchColor.ToLower()} witch!";
        SetTransparent(true);
    }

    void SetTransparent(bool transparent) {
        Color temp = image.color;

        if (transparent) {
            temp.a = .4f;
        } else {
            temp.a = 1;
        }

        image.color = temp;
    }

    private string GetInputType() {
        if (witchColor.Equals("Pink"))
            return "Q";
        return "SPACE";
    }
}
