using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInformation
{
    public static List<Boss> defeatedBosses = new List<Boss>();

    public static string playerTag = "Player";
    public static string playerSpellTag = "PlayerProjectile";
    public static string playerOneTag = "Player1";
    public static string playerTwoTag = "Player2";

    public static string enemyTag = "Enemy";
    public static string enemyBulletTag = "EnemyProjectile";

    public static Color purple = new Color32(233, 143, 255, 255);
    public static Color pink = new Color32(255, 153, 153, 255);
    public static Color indestructibleColor = new Color32(245, 245, 245, 255);
    public static Color nullColor = Color.white;

    public static string pumpkinScene = "Pumpkin";
    public static string ghostScene = "Ghost";
    public static string skeletonScene = "Skeleton";
    public static string frankensteinScene = "Frankenstein";
    public static string catScene = "Cat";
    public static string vampireScene = "Vampire";
    public static string scientistScene = "Scientist";
    public static string grumwaldaScene = "Grumwalda";

    public static string deathScene = "GameOver";
    public static string winScene = "UniversalWinScreen";
    public static string menuScene = "MainMenu";
    public static string levelSelectScene = "LevelSelect";
    public static string loadingScreenScene = "LoadingScreen";
    public static string creditsScene = "Credits";

    public static string tutorial1 = "KyleTutorial";
    public static string tutorial2 = "KyleTutorial2";

    public static bool IsFinger(string name) {
        return name.Equals("Index") ||
            name.Equals("Thumb") ||
            name.Equals("Middle") ||
            name.Equals("Ring") ||
            name.Equals("Pinkie");
    }
}

public enum Boss {
    PUMPKIN,
    GHOST,
    SKELETON,
    FRANKENSTEIN,
    CAT,
    VAMPIRE,
    SCIENTIST,
    GRUMWALDA
}