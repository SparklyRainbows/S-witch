using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region audioclips
    [SerializeField]
    [Tooltip("Main BGM")]
    AudioClip mainBGM;

    [SerializeField]
    [Tooltip("Player death BGM")]
    AudioClip playerDeathBGM;
    #endregion

    private AudioSource audio;

    private void Start() {
        audio = GetComponent<AudioSource>();
    }

    public void PlayerDeath() {
        audio.clip = playerDeathBGM;
        audio.loop = false;
        audio.Play();
    }

    public void MainBGM() {
        if (audio.clip.name.Equals(mainBGM.name))
            return;

        audio.clip = mainBGM;
        audio.loop = true;
        audio.Play();
    }
}
