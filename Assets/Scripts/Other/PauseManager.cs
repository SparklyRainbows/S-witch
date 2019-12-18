using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
    private GameObject pauseUI;

    private GameObject help;

    private Button resume;
    private Button helpButton;
    private Button levelSelect;
    private Button menu;

    public Sprite pumpkinHelp;
    public Sprite ghostHelp;
    public Sprite scientistHelp;

    private void Start() {
        InitButtons();

        pauseUI = GameObject.Find("PauseUI");
        SetPauseUI(false);

        InitHelp();
        SetHelp(false);
    }

    private void Update() {
        if (IsHelpOpen() && Input.GetButtonDown("Cancel")) {
            SetHelp(false);
        }
    }

    private void InitButtons() {
        resume = GameObject.Find("Resume").GetComponent<Button>();
        helpButton = GameObject.Find("Help").GetComponent<Button>();
        levelSelect = GameObject.Find("LevelSelect").GetComponent<Button>();
        menu = GameObject.Find("Menu").GetComponent<Button>();

        resume.onClick.AddListener(GameManager.instance.TogglePause);
        levelSelect.onClick.AddListener(GameManager.instance.ToLevelSelect);
        menu.onClick.AddListener(GameManager.instance.ToMenu);
        helpButton.onClick.AddListener(delegate { SetHelp(true); });
    }

    public void SetPauseUI(bool show) {
        pauseUI.SetActive(show);
    }

    #region help ui
    public void SetHelp(bool show) {
        help.SetActive(show);
    }

    private void InitHelp() {
        help = GameObject.Find("HelpPanel");

        Boss boss = GameManager.instance.GetCurrentBoss();
        switch(boss) {
            case Boss.PUMPKIN:
                SetHelpImage(pumpkinHelp);
                return;
            case Boss.GHOST:
                SetHelpImage(ghostHelp);
                return;
            case Boss.SCIENTIST:
                SetHelpImage(scientistHelp);
                return;
            default:
                Debug.LogWarning($"No help screen found for {boss}");
                return;
        }
    }

    private void SetHelpImage(Sprite s) {
        help.GetComponent<Image>().sprite = s;
    }

    public bool IsHelpOpen() {
        return help.activeSelf;
    }
    #endregion
}
