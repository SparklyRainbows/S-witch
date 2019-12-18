using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject lockIcon;

    [SerializeField]
    private Boss boss;
    private int numReady;

    private bool canSelect = true;

    private void Start() {
        if (lockIcon) {
            lockIcon.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (GameInformation.defeatedBosses.Contains(boss)) {
            GetComponent<SpriteRenderer>().color = Color.gray;
            canSelect = false;
        }

        if (boss == Boss.GRUMWALDA) {
            if (GameInformation.defeatedBosses.Count < 7) {
                lockIcon.GetComponent<SpriteRenderer>().enabled = true;
                canSelect = false;
            }
        }

        if (GameInformation.defeatedBosses.Count == 0 && boss != Boss.PUMPKIN) {
            lockIcon.GetComponent<SpriteRenderer>().enabled = true;
            canSelect = false;
        }

        numReady = 0;
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.gameObject.CompareTag(GameInformation.playerSpellTag)) {
    //        LoadScene();
    //    }
    //}

    private void LoadScene() {
        GameManager.instance.SetCurrentBoss(boss);
        SceneManagement.LoadingScreen();
    }

    public void Ready()
    {
        numReady += 1;
        if (numReady == 2)
        {
            LoadScene();
        }
        StartCoroutine(ReadyFlash());
    }

    public void Unready()
    {
        numReady -= 1;
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private IEnumerator ReadyFlash()
    {
        float flashDelay = .2f;
        while (true)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            GetComponent<SpriteRenderer>().color = Color.green;
            yield return new WaitForSeconds(flashDelay);

            GetComponent<SpriteRenderer>().color = Color.white;
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(flashDelay);
        }
    }

    public bool CanSelect() {
        return canSelect;
    }
}
