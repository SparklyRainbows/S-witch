using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    #region image and text vars
    [Header("Boss Poems")]
    public string[] pumpkinPoem;
    public string[] ghostPoem;
    public string[] skeletonPoem;
    public string[] frankensteinPoem;
    public string[] catPoem;
    public string[] vampirePoem;
    public string[] scientistPoem;
    public string[] grumwaldaPoem;

    [Header("Voice lines")]
    public AudioClip[] pumpkinClips;
    public AudioClip[] ghostClips;
    public AudioClip[] skeletonClips;
    public AudioClip[] frankensteinClips;
    public AudioClip[] catClips;
    public AudioClip[] vampireClips;
    public AudioClip[] scientistClips;
    public AudioClip[] grumwaldaClips;

    [Header("Boss Images")]
    public Sprite pumpkin;
    public Sprite ghost;
    public Sprite skeleton;
    public Sprite frankenstein;
    public Sprite cat;
    public Sprite vampire;
    public Sprite scientist;
    public Sprite grumwalda;
    #endregion

    [Header("UI Objects")]
    public Text poemText;
    public Image bossImage;
    public Image blackscreen;

    private AudioSource audio;

    private Boss boss;
    private string[] poem;
    private Queue<AudioClip> audioClip;

    private float characterDisplayDelay = .03f;

    private bool skipped;

    private void Start() {
        audio = GetComponent<AudioSource>();

        boss = GameManager.instance.GetCurrentBoss();

        SetImage();
        SetText();
        SetAudio();

        StartCoroutine(DisplayPoem());
    }

    private void Update() {
        if (skipped) {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Q)) {
            skipped = true;
            StopAllCoroutines();

            StartCoroutine(SkipPoem());
        }
    }

    #region init funcs
    private void SetImage() {
        switch (boss) {
            case Boss.PUMPKIN:
                bossImage.sprite = pumpkin;
                break;
            case Boss.GHOST:
                bossImage.sprite = ghost;
                break;
            case Boss.SKELETON:
                bossImage.sprite = skeleton;
                break;
            case Boss.FRANKENSTEIN:
                bossImage.sprite = frankenstein;
                break;
            case Boss.CAT:
                bossImage.sprite = cat;
                break;
            case Boss.VAMPIRE:
                bossImage.sprite = vampire;
                break;
            case Boss.SCIENTIST:
                bossImage.sprite = scientist;
                break;
            case Boss.GRUMWALDA:
                bossImage.sprite = grumwalda;
                break;
            default:
                Debug.LogWarning($"Boss not found: {boss}");
                bossImage.sprite = null;
                break;
        }
    }

    private void SetText() {
        switch (boss) {
            case Boss.PUMPKIN:
                poem = pumpkinPoem;
                break;
            case Boss.GHOST:
                poem = ghostPoem;
                break;
            case Boss.SKELETON:
                poem = skeletonPoem;
                break;
            case Boss.FRANKENSTEIN:
                poem = frankensteinPoem;
                break;
            case Boss.CAT:
                poem = catPoem;
                break;
            case Boss.VAMPIRE:
                poem = vampirePoem;
                break;
            case Boss.SCIENTIST:
                poem = scientistPoem;
                break;
            case Boss.GRUMWALDA:
                poem = grumwaldaPoem;
                break;
            default:
                Debug.LogWarning($"Boss not found: {boss}");
                break;
        }
    }

    private void SetAudio() {
        switch (boss) {
            case Boss.PUMPKIN:
                audioClip = new Queue<AudioClip>(pumpkinClips);
                break;
            case Boss.GHOST:
                audioClip = new Queue<AudioClip>(ghostClips);
                break;
            case Boss.SKELETON:
                audioClip = new Queue<AudioClip>(skeletonClips);
                break;
            case Boss.FRANKENSTEIN:
                audioClip = new Queue<AudioClip>(frankensteinClips);
                break;
            case Boss.CAT:
                audioClip = new Queue<AudioClip>(catClips);
                break;
            case Boss.VAMPIRE:
                audioClip = new Queue<AudioClip>(vampireClips);
                break;
            case Boss.SCIENTIST:
                audioClip = new Queue<AudioClip>(scientistClips);
                break;
            case Boss.GRUMWALDA:
                audioClip = new Queue<AudioClip>(grumwaldaClips);
                break;
            default:
                Debug.LogWarning($"Boss not found: {boss}");
                break;
        }
    }
    #endregion

    #region display text funcs
    private IEnumerator DisplayPoem() {
        poemText.text = "";

        foreach (string line in poem) {
            PlayAudio();
            yield return DisplayLine(line);

            poemText.text += "\n";
        }

        //yield return new WaitForSeconds(2);

        yield return FadeToBlack();

        StartGame();
    }

    private IEnumerator DisplayLine(string line) {
        Queue<char> letters = new Queue<char>();
        foreach (char c in line.ToCharArray()) {
            letters.Enqueue(c);
        }

        while (letters.Count > 0) {
            yield return DisplayLetter(letters.Dequeue());
        }

        while (audio.isPlaying) {
            yield return null;
        }
    }

    private IEnumerator DisplayLetter(char c) {
        poemText.text += c;
        yield return new WaitForSeconds(characterDisplayDelay);
    }

    private IEnumerator FadeToBlack() {
        float alpha = 0;
        while (alpha < 1) {
            alpha += .05f;

            Color c = blackscreen.color;
            c.a = alpha;
            blackscreen.color = c;

            yield return null;
        }
    }
    #endregion

    #region audio funcs
    private void PlayAudio() {
        audio.clip = audioClip.Dequeue();
        audio.Play();
    }

    private float GetClipLength() {
        return audio.clip.length;
    }
    #endregion

    public void StartGame() {
        GameManager.instance.StartGame();

        switch (boss) {
            case Boss.PUMPKIN:
                SceneManagement.Pumpkin();
                return;
            case Boss.GHOST:
                SceneManagement.Ghost();
                return;
            case Boss.SKELETON:
                SceneManagement.Skeleton();
                return;
            case Boss.FRANKENSTEIN:
                SceneManagement.Frankenstein();
                return;
            case Boss.CAT:
                SceneManagement.Cat();
                return;
            case Boss.VAMPIRE:
                SceneManagement.Vampire();
                return;
            case Boss.SCIENTIST:
                SceneManagement.Scientist();
                return;
            case Boss.GRUMWALDA:
                SceneManagement.Grumwalda();
                return;
            default:
                Debug.LogError($"Boss not found: {boss}");
                return;
        }
    }

    private IEnumerator SkipPoem() {
        yield return FadeToBlack();
        StartGame();
    }
}
