using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UniversalWinScreen : MonoBehaviour
{
    public Image blackscreen;
    public Image image;

    private void Start() {
        if (SceneManager.GetActiveScene().name.Equals(GameInformation.winScene)) {
            StartCoroutine(ShowImage());
        } else {

            if (GameManager.instance.PlayBossVanquished()) {
                StartCoroutine(FadeImage());
            } else {
                blackscreen.enabled = false;
                image.enabled = false;
            }
        }
    }

    private IEnumerator ShowImage() {
        float alpha = 0;
        while (alpha < 1) {
            alpha += .05f;

            Color c = image.color;
            c.a = alpha;
            image.color = c;

            yield return null;
        }

        yield return new WaitForSeconds(3f);

        SceneManagement.LevelSelect();
    }

    private IEnumerator FadeImage() {
        blackscreen.enabled = false;

        float alpha = 1;
        while (alpha > 0) {
            alpha -= .01f;

            Color c = image.color;
            c.a = alpha;
            image.color = c;

            yield return null;
        }
    }
}
