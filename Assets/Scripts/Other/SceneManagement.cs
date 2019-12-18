using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagement
{
    public static string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }

    public static void Pumpkin() {
        SceneManager.LoadScene(GameInformation.pumpkinScene);
    }

    public static void Ghost() {
        SceneManager.LoadScene(GameInformation.ghostScene);
    }

    public static void Skeleton() {
        SceneManager.LoadScene(GameInformation.skeletonScene);
    }

    public static void Frankenstein() {
        SceneManager.LoadScene(GameInformation.frankensteinScene);
    }

    public static void Cat() {
        SceneManager.LoadScene(GameInformation.catScene);
    }

    public static void Vampire() {
        SceneManager.LoadScene(GameInformation.vampireScene);
    }

    public static void Scientist() {
        SceneManager.LoadScene(GameInformation.scientistScene);
    }

    public static void Grumwalda() {
        SceneManager.LoadScene(GameInformation.grumwaldaScene);
    }

    public static void LoadingScreen() {
        SceneManager.LoadScene(GameInformation.loadingScreenScene);
    }

    public static void GameOver() {
        SceneManager.LoadScene(GameInformation.deathScene);
    }

    public static void Win() {
        if (GameManager.instance.gameWon) {
            SceneManager.LoadScene(GameInformation.creditsScene);
        } else {
            SceneManager.LoadScene(GameInformation.winScene);
        }
    }

    public static void MainMenu() {
        SceneManager.LoadScene(GameInformation.menuScene);
        GameManager.instance.StartGame();
    }

    public static void LevelSelect() {
        SceneManager.LoadScene(GameInformation.levelSelectScene);
    }

    public static void Tutorial1()
    {
        SceneManager.LoadScene(GameInformation.tutorial1);
    }

    public static void Tutorial2()
    {
        SceneManager.LoadScene(GameInformation.tutorial2);
    }
}
