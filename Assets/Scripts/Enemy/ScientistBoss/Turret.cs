using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("the nub rotating around the turrent")]
    private GameObject turretNub;

    [SerializeField]
    [Tooltip("how fast turret shoots per second")]
    private float fireRate;

    [SerializeField]
    [Tooltip("how much health the turret has")]
    private float totalHealth;

    [SerializeField]
    [Tooltip("velocity of the turret when it moves")]
    private Vector3 moveVelocity;

    [SerializeField]
    [Tooltip("how much faster turrets shoot at round 2")]
    private float shootMultiplier;

    [SerializeField]
    [Tooltip("how fast the forcefield turns on and off")]
    private float colorChangeTime;

    public GameObject turretBase;
    public GameObject turretHead;

    [Header("SFX")]
    public AudioClip shoot;
    public AudioClip hurt;
    public AudioClip die;
    #endregion

    #region Private Variables
    private Color turretColor;
    private Color currentColor;
    private float currentHealth;
    private bool takingDamage;
    private bool dead;
    private bool shooting;
    private bool phase2;
    private GameObject bullet;

    private int phase;

    private AudioSource audio;
    #endregion

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        phase = 1;
        phase2 = false;
        currentHealth = totalHealth;
        StartCoroutine(Shooter());
        shooting = true;
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        if (phase == 3 && !dead)
        {
            Move();
        }
    }
    #endregion

    #region Shooting Methods
    public IEnumerator Shooter()
    {
        while (true)
        {
            if (GameManager.instance.IsGameOver())
            {
                break;
            }
            if (shooting)
            {
                PlayShoot();
                Shoot();
            }
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bullet, turretNub.transform.position, turretNub.transform.localRotation);
        bulletObj.GetComponent<EnemyBullet>().SetColor(turretColor);
    }

    public void SetBullet(GameObject bulletObj)
    {
        bullet = bulletObj;
    }
    #endregion;

    #region Turret State Functions
    public void ResetTurret()
    {
        StartCoroutine(Reset(turretColor));
        currentHealth = totalHealth;
        dead = false;
        turretNub.GetComponent<RotateAroundSemi>().StartRotating();
        shooting = true;
    }

    private IEnumerator Reset(Color nextColor)
    {
        float elapsedTime = 0.0f;
        Color elapsedColor = currentColor;
        while (elapsedTime < colorChangeTime)
        {
            elapsedTime += Time.deltaTime;
            elapsedColor = Color.Lerp(elapsedColor, nextColor, elapsedTime / colorChangeTime);
            turretHead.GetComponent<SpriteRenderer>().color = nextColor;
            turretBase.GetComponent<SpriteRenderer>().color = nextColor;
            yield return null;
        }
        currentColor = nextColor;
    }

    private void Die()
    {
        PlayDie();

        ChangeColor(GameInformation.nullColor);
        shooting = false;
        dead = true;
        turretNub.GetComponent<RotateAroundSemi>().StopRotating();
    }
    public bool IsDead()
    {
        return dead;
    }

    public void NextPhase()
    {
        phase += 1;
        if (phase == 2 && !phase2)
        {
            phase2 = true;
            fireRate = fireRate * shootMultiplier;

        }
    }
    #endregion

    #region Color Functions
    public void SetColor(Color c)
    {
        turretColor = c;
        currentColor = c;
        turretHead.GetComponent<SpriteRenderer>().color = c;
        turretBase.GetComponent<SpriteRenderer>().color = c;
    }

    public void ChangeColor(Color c)
    {
        currentColor = c;
        turretHead.GetComponent<SpriteRenderer>().color = c;
        turretBase.GetComponent<SpriteRenderer>().color = c;
    }

    public Color GetColor()
    {
        return currentColor;
    }
    #endregion

    #region Damage Functions
    public void TakeDamage(float damage)
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }

        PlayHurt();

        currentHealth -= damage;

        if (!takingDamage)
        {
            StartCoroutine(TakeDamage());
        }
    }

    protected IEnumerator TakeDamage()
    {
        takingDamage = true;
        Color prevColor1 = turretBase.GetComponent<SpriteRenderer>().color;
        Color prevColor2 = turretHead.GetComponent<SpriteRenderer>().color;

        float flashDelay = .1f;
        for (int i = 0; i < 3; i++)
        {
            turretBase.GetComponent<SpriteRenderer>().color = Color.red;
            turretHead.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(flashDelay);

            turretBase.GetComponent<SpriteRenderer>().color = prevColor1;
            turretHead.GetComponent<SpriteRenderer>().color = prevColor2;
            yield return new WaitForSeconds(flashDelay);
        }
        takingDamage = false;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    #endregion

    #region Collision Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag))
        {
            if (collision.gameObject.GetComponent<Bullet>().IsColor(currentColor) && currentColor != GameInformation.nullColor)
            {
                TakeDamage(collision.GetComponent<PlayerBullet>().damage);
                Instantiate(collision.GetComponent<PlayerBullet>().bigBurst, collision.transform.position, Quaternion.identity);
            }
            else if (dead)
            {
                Instantiate(collision.GetComponent<PlayerBullet>().lilBurst, collision.transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerUlt"))
        {
            TakeDamage(collision.gameObject.GetComponent<Beam>().damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorizontalWall") || collision.gameObject.CompareTag("Turret"))
        {
            moveVelocity.y *= -1;
        }

    }
    #endregion

    #region Movement Functions
    private void Move()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = moveVelocity;
    }
    #endregion

    #region Audio Functions
    private void PlayShoot() {
        audio.clip = shoot;
        audio.Play();
    }

    private void PlayHurt() {
        audio.clip = hurt;
        audio.Play();
    }

    private void PlayDie() {
        audio.clip = die;
        audio.Play();
    }
    #endregion
}
