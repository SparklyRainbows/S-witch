using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManagement.Tutorial1();
    }

    public void LevelSelect() {
        SceneManagement.LevelSelect();
    }

    public void MainMenu() {
        SceneManagement.MainMenu();
    }

    public void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
		Application.Quit();
        #endif
    }
}
