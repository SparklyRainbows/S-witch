using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBehavior : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The witch which flys around")]
    GameObject m_innerWitch;

    [SerializeField]
    [Tooltip("The witch which shoots around")]
    GameObject m_outerWitch;

    [SerializeField]
    [Tooltip("Total player health")]
    float maxHealth;

    public float witchDistance;
    #endregion

    #region Switch Variables
    private float timeBetweenSwitch;
    private float perfSwitchTime = 0.1f;
    private bool perfectSwitch;
    private bool flashing = false;

    [SerializeField]
    [Tooltip("Switch Slider")]
    private GameObject switchSlider;

    [SerializeField]
    [Tooltip("Particles for perfect switch")]
    private ParticleSystem switchParticles;
    #endregion

    #region Health_variables
    private float curHealth;
    private Slider healthSlider;
    private bool takingDamage;
    #endregion

    #region MP_variables
    private float currentCharge = 0f;
    private float switchChargeAmount = 7f;
    private float passiveChargeAmount = 2f;
    private float maxCharge = 100f;
    private Slider MPSlider;
    private Text mpAmount;

    [SerializeField]
    [Tooltip("BEAM")]
    private GameObject beam;

    [SerializeField]
    [Tooltip("the shockwave attack")]
    private GameObject wave;

    [SerializeField]
    [Tooltip("all the spells the player can cast")]
    private GameObject[] spells;

    private GameObject currentSpell;
    private float currentSpellCost;
    private int currentSpellIndex;
    private int numSpells;
    private Image spellSprite;
    #endregion

    #region SFX_variables
    [Header("SFX")]
    public AudioClip shoot;
    public AudioClip hurt;
    public AudioClip die;
    public AudioClip heal;
    public AudioClip switchSound;

    private AudioSource audio;
    #endregion

    #region Initialization
    private void Awake()
    {
        audio = GetComponent<AudioSource>();

        perfectSwitch = false;
        numSpells = spells.Length;
        currentSpellIndex = 0;
        currentSpell = spells[currentSpellIndex];
        m_innerWitch.GetComponent<PlayerController>().inside = true;
        m_outerWitch.GetComponent<PlayerController>().inside = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        try {
            healthSlider = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
            MPSlider = GameObject.Find("PlayerUltBar").GetComponent<Slider>();
            spellSprite = GameObject.Find("SpellSprite").GetComponent<Image>();
            spellSprite = GameObject.Find("SpellSprite").GetComponent<Image>();
            mpAmount = GameObject.Find("MPAmount").GetComponent<Text>();
            currentSpellCost = currentSpell.GetComponent<Spell>().MPCost();
            spellSprite.sprite = currentSpell.GetComponent<Spell>().GetSprite();
        } catch (NullReferenceException e) {
            Debug.LogWarning("Couldn't find healthSlider/ultSlider");
        }
        
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPaused && !SceneManagement.GetCurrentScene().Equals(GameInformation.tutorial2)) {
            return;
        }

        if (currentCharge < maxCharge)
        {
            ChargeMP(passiveChargeAmount * Time.deltaTime);
        }

        if (Input.GetButtonDown("NextSpell"))
        {
            SetNextSpell(1);
        }
        else if (Input.GetButtonDown("PrevSpell"))
        {
            SetNextSpell(-1);
        }


        if (!m_innerWitch.GetComponent<PlayerController>().ReadyToSwitch() && 
        !m_outerWitch.GetComponent<PlayerController>().ReadyToSwitch() && 
        m_innerWitch.GetComponent<SpriteRenderer>().color == Color.magenta)
        {
            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        } 

        if (m_innerWitch.GetComponent<PlayerController>().ReadyToSwitch()
        && m_outerWitch.GetComponent<PlayerController>().ReadyToSwitch())
        {
            if (switchSlider.GetComponent<SwitchSlider>().PerfectSwitch())
            {
                StartCoroutine(PerfSwitch());
            }
            Time.timeScale = 1f;
            Switch();       
            m_innerWitch.GetComponent<PlayerController>().Unready();
            m_outerWitch.GetComponent<PlayerController>().Unready();
            timeBetweenSwitch = 0.0f;

            switchSlider.SetActive(false);
        }
        else if (m_innerWitch.GetComponent<PlayerController>().ReadyToSwitch()
        || m_outerWitch.GetComponent<PlayerController>().ReadyToSwitch())
        {
            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.magenta;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.magenta;
            StartCoroutine(PerfSwitchTimer());
            Time.timeScale = .1f;
            switchSlider.SetActive(true);
        }
        else if (!m_innerWitch.GetComponent<PlayerController>().ReadyToSwitch()
        && !m_outerWitch.GetComponent<PlayerController>().ReadyToSwitch()
        && !GameManager.instance.isPaused)
        {

            Time.timeScale = 1f;
            switchSlider.SetActive(false);
        }
        else if (currentCharge < maxCharge)
        {
            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (m_innerWitch.GetComponent<PlayerController>().ReadyToAttack()
        && m_outerWitch.GetComponent<PlayerController>().ReadyToAttack()
        && currentCharge >= currentSpellCost)
        {
            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;

            if (!MPSlider)
                return;

            Instantiate(currentSpell, transform);
            currentCharge -= currentSpellCost;
        }
        else if ((m_innerWitch.GetComponent<PlayerController>().ReadyToAttack()
        || m_outerWitch.GetComponent<PlayerController>().ReadyToAttack())
        && currentCharge >= currentSpellCost
        && MPSlider != null)
        {
            if (!flashing) StartCoroutine(UltFlash());
            flashing = true;
        }
        else if (currentCharge >= currentSpellCost && !flashing && MPSlider)
        {
            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.cyan;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        //if (m_innerWitch.GetComponent<PlayerController>().ReadyToWave() 
        //&& currentCharge >= wave.GetComponent<Spell>().MPCost())
        //{
        //    Instantiate(wave, transform);
        //    currentCharge -= wave.GetComponent<Spell>().MPCost();
        //}
        if (mpAmount != null) {
            mpAmount.text = Mathf.RoundToInt(currentCharge).ToString();
        }
    }
    #endregion

    #region Switch Function
    public void Switch()
    {
        PlaySwitch();
        GetComponent<Animator>().SetTrigger("poof");

        ChargeMP(switchChargeAmount);

/*        m_innerWitch.GetComponent<PlayerController>().StopRotating();
*/      m_innerWitch.GetComponent<PlayerController>().Switch();
        m_outerWitch.GetComponent<PlayerController>().Switch();
        GameObject oldInner = m_innerWitch;
        Vector3 innerPos = m_innerWitch.transform.position;
        m_innerWitch.transform.position = m_outerWitch.transform.position;
        m_innerWitch.transform.rotation = m_outerWitch.transform.rotation;
        m_outerWitch.transform.position = innerPos;
        m_outerWitch.transform.rotation = Quaternion.identity;
        m_innerWitch = m_outerWitch;
        m_outerWitch = oldInner;
    }

    IEnumerator PerfSwitchTimer()
    {
        timeBetweenSwitch = 0.0f;
        while (timeBetweenSwitch < perfSwitchTime)
        {
            timeBetweenSwitch += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PerfSwitch()
    {
        Instantiate(switchParticles, transform.position, Quaternion.identity);

        float flashDelay = .1f;
        perfectSwitch = true;
        m_innerWitch.GetComponent<SpriteRenderer>().color = Color.green;
        m_outerWitch.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(flashDelay);

        perfectSwitch = false;
        m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(flashDelay);
    }
    public bool CompletedPerfSwitch()
    {
        return perfectSwitch;
    }
    #endregion

    #region Damage Functions
    public void TakeDamage(float value)
    {
        //If you're still in the red flash animation, you have Iframes
        if (takingDamage) {
            return;
        }

        PlayHurt();

        curHealth -= value;

        healthSlider.value = curHealth / maxHealth;

        if (curHealth <= 0) {
            Die();
        }

        StartCoroutine(TakeDamage());
    }

    private IEnumerator TakeDamage() {
        takingDamage = true;

        /*foreach (Collider2D col in GetComponentsInChildren<Collider2D>()) {
            col.enabled = false;
        }*/

        float flashDelay = .1f;
        for (int i = 0; i < 3; i++) {
            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.red;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(flashDelay);

            m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
            m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(flashDelay);
        }

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>()) {
            col.enabled = true;
        }

        takingDamage = false;
    }

    public void Die()
    {
        //Destroy(m_innerWitch);
        //Destroy(m_outerWitch);
        //Destroy(gameObject);
        GameManager.instance.GameOver();
        SceneManagement.GameOver();
    }

    public bool TakingDamage() {
        return takingDamage;
    }
    #endregion

    #region Healing functions
    public void GainHealth(float amount) {
        PlayHeal();

        curHealth += amount;
        if (curHealth > maxHealth) {
            curHealth = maxHealth;
        }

        healthSlider.value = curHealth / maxHealth;
    }
    #endregion

    #region Ult functions
    public void ChargeMP(float amount) {
        if (!MPSlider) {
            return;
        }
        currentCharge += amount;
        if(currentCharge > maxCharge)
        {
            currentCharge = maxCharge;
        }
        MPSlider.value = currentCharge / maxCharge;
    }

    IEnumerator UltFlash()
    {
        print("flashing");
        m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(.1f);
        m_innerWitch.GetComponent<SpriteRenderer>().color = Color.cyan;
        m_outerWitch.GetComponent<SpriteRenderer>().color = Color.cyan;
        yield return new WaitForSeconds(.1f);
        flashing = false;
        print("ready to flash again");
    }

    private void SetNextSpell(int dir)
    {
        currentSpellIndex = (currentSpellIndex + dir) % numSpells;
        if (currentSpellIndex == -1)
        {
            currentSpellIndex += numSpells;
        }

        currentSpell = spells[currentSpellIndex];

        try {
            currentSpellCost = currentSpell.GetComponent<Spell>().MPCost();
            spellSprite.sprite = currentSpell.GetComponent<Spell>().GetSprite();

        } catch (Exception e) {
            Debug.LogWarning("Spell sprite component not set");
        }

        m_innerWitch.GetComponent<SpriteRenderer>().color = Color.white;
        m_outerWitch.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Audio functions
    public void PlayShoot() {
        if (audio.isPlaying && audio.clip != shoot) {
            return;
        }
        audio.clip = shoot;
        audio.Play();
    }

    public void PlayHurt() {
        audio.Stop();
        audio.clip = hurt;
        audio.Play();
    }

    public void PlayDie() {
        audio.Stop();
        audio.clip = die;
        audio.Play();
    }

    public void PlayHeal() {
        audio.Stop();
        audio.clip = heal;
        audio.Play();
    }

    public void PlaySwitch() {
        audio.Stop();
        audio.clip = switchSound;
        audio.Play();
    }
    #endregion
}
