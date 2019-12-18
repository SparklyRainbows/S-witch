using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameWon;

    public static GameManager instance = null;
    private SoundManager sm;

    private bool gameover;

    private Boss currentBoss;
    private Color currentBossColor;

    public bool isPaused;
    private PauseManager pm;

    //If you enter the level select from the start/menu/pause screen, don't play the boss vanquished animation
    private bool playBossVanquished;

    #region initialization
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void Start() {
        sm = GetComponent<SoundManager>();
    }
    #endregion

    private void Update() {
        if (Input.GetButtonDown("pause")) {
            TogglePause();
        }
    }

    public void RestartGame() {
        GameInformation.defeatedBosses = new List<Boss>();
        gameover = false;
        gameWon = false;
        playBossVanquished = false;
    }

    #region pause funcs
    public void TogglePause() {
        try {
            pm = GameObject.Find("UI").GetComponent<PauseManager>();

            if (pm.IsHelpOpen()) {
                return;
            }

            isPaused = !isPaused;
            pm.SetPauseUI(isPaused);

            if (isPaused) {
                Time.timeScale = 0f;
            } else {
                Time.timeScale = 1f;
            }

        } catch {
            Debug.LogWarning("Couldn't find pause manager");
        }
    }

    public void ToLevelSelect() {
        Time.timeScale = 1f;
        isPaused = false;
        pm.SetPauseUI(isPaused);

        SceneManagement.LevelSelect();
    }

    public void ToMenu() {
        Time.timeScale = 1f;
        isPaused = false;
        pm.SetPauseUI(isPaused);

        SceneManagement.MainMenu();
    }
    #endregion

    #region game state funcs

    public void GameOver() {
        gameover = true;
        sm.PlayerDeath();
    }

    public void Win() {
        gameover = true;
        GameInformation.defeatedBosses.Add(currentBoss);

        playBossVanquished = true;
    }

    public void StartGame() {
        gameover = false;
        sm.MainBGM();
    }

    public bool IsGameOver() {
        return gameover;
    }
    #endregion

    #region boss funcs
    public void SetCurrentBoss(Boss boss) {
        currentBoss = boss;
    }

    public Boss GetCurrentBoss() {
        return currentBoss;
    }

    public void SetBossColor(Color color) {
        currentBossColor = color;
    }

    public Color GetBossColor() {
        return currentBossColor;
    }

    public bool PlayBossVanquished() {
        return playBossVanquished;
    }
    #endregion
}
