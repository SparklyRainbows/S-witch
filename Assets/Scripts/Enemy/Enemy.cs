using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public ParticleSystem deathParticles;

    [Header("SFX")]
    public AudioClip shoot;
    public AudioClip hurt;
    public AudioClip die;

    protected Animator animator;
    protected SpriteRenderer renderer;
    protected AudioSource audio;

    protected Image blackscreen;

    #region attack_vars
    [Header("Other")]
    public GameObject bulletObj;
    #endregion

    #region health_and_damage_vars
    protected float totalHealth;
    protected float currentHealth;

    protected Slider healthSlider;
    protected bool takingDamage;
    protected string deathClip;
    #endregion

    #region Colors
    protected Color currentColor;
    #endregion 

    protected virtual void Start() {
        currentHealth = totalHealth;
        healthSlider = GameObject.Find("EnemyHealthBar").GetComponent<Slider>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        blackscreen = GameObject.Find("BlackScreen").GetComponent<Image>();

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(StartFight());
    }

    #region damage_functions
    public virtual void TakeDamage(float damage) {
        if (GameManager.instance.IsGameOver()) {
            return;
        }

        PlayHurt();

        currentHealth -= damage;
        healthSlider.value = currentHealth / totalHealth;

        if (!takingDamage) {
            StartCoroutine(TakeDamage());
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    protected virtual IEnumerator TakeDamage() {
        takingDamage = true;

        float flashDelay = .1f;
        for (int i = 0; i < 3; i++) {
            renderer.color = Color.red;
            yield return new WaitForSeconds(flashDelay);

            renderer.color = GameManager.instance.GetBossColor();
            yield return new WaitForSeconds(flashDelay);
        }

        takingDamage = false;
    }

    protected virtual void Die() {
        PlayDie();
        StartCoroutine(DeathAnimation());
    }

    protected virtual IEnumerator DeathAnimation() {
        //animator.SetBool("dead", true);
        GetComponent<Collider2D>().enabled = false;
        GameManager.instance.Win();

        /*AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips) {
            if (clip.name.Equals(deathClip)) {
                yield return new WaitForSeconds(clip.length * 3f);
            }
        }*/

        GameObject.Find("Main Camera").GetComponent<ScreenShake>().Shake();
        StartCoroutine(SpawnDeathParticles());
        yield return new WaitForSeconds(2.5f);
        yield return FadeToBlack();

        Destroy(gameObject);
        SceneManagement.Win();
    }

    protected IEnumerator SpawnDeathParticles() {
        Vector2 pos = transform.position;
        float varianceAmount = 1.5f;

        int particlesToSpawn = 10;
        for (int i = 0; i < particlesToSpawn; i++) {
            Vector2 spawnPos = new Vector2(Random.Range(pos.x - varianceAmount, pos.x + varianceAmount),
                Random.Range(pos.y - varianceAmount, pos.y + varianceAmount));

            Instantiate(deathParticles, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(3f / particlesToSpawn);
        }
    }

    protected IEnumerator FadeToBlack() {
        blackscreen = GameObject.Find("BlackScreen").GetComponent<Image>();

        float alpha = 0;
        while (alpha < 1) {
            alpha += .05f;

            Color c = blackscreen.color;
            c.a = alpha;
            blackscreen.color = c;

            yield return new WaitForSeconds(.05f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("PlayerUlt")) {
            TakeDamage(collision.gameObject.GetComponent<Beam>().damage);
        }
    }
    #endregion

    protected virtual IEnumerator StartFight() {
        float delayTime = 1f;

        GameObject readyPanelObj = GameObject.Find("ReadyPanel");
        readyPanelObj.GetComponent<Image>().enabled = true;
        readyPanelObj.GetComponentInChildren<Text>().enabled = true;
        readyPanelObj.GetComponentInChildren<Text>().text = "READY";
        yield return new WaitForSeconds(delayTime);
        readyPanelObj.GetComponentInChildren<Text>().text = "GO!";
        yield return new WaitForSeconds(delayTime);
        readyPanelObj.GetComponent<Image>().enabled = false;
        readyPanelObj.GetComponentInChildren<Text>().enabled = false;

        GetComponent<Collider2D>().enabled = true;

        StartCoroutine(Attack());
    }

    public virtual Color GetCurrentEnemyColor()
    {
        return currentColor;
    }

    protected virtual IEnumerator Attack() {
        yield return null;
    }

    #region audio_functions
    protected virtual void PlayShoot() {
        audio.clip = shoot;
        audio.Play();
    }

    protected virtual void PlayHurt() {
        audio.clip = hurt;
        audio.Play();
    }

    protected virtual void PlayDie() {
        audio.clip = die;
        audio.Play();
    }
    #endregion
}
