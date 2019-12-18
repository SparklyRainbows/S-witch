using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsRoll : MonoBehaviour
{
    public GameObject creditsText;
    public Image blackscreen;

    private float endingPos = 970f;
    private float scrollAmount = 2f;

    private void Start() {
        StartCoroutine(RollCredits());
    }

    private IEnumerator RollCredits() {
        while (creditsText.transform.position.y < endingPos) {
            Vector2 pos = creditsText.transform.position;
            pos.y += scrollAmount;
            creditsText.transform.position = pos;

            yield return null;
        }

        yield return new WaitForSeconds(3);

        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack() {
        float alpha = 0;
        while (alpha < 1) {
            alpha += .03f;

            Color c = blackscreen.color;
            c.a = alpha;
            blackscreen.color = c;

            yield return null;
        }

        GameManager.instance.RestartGame();
        SceneManagement.MainMenu();
    }
}
