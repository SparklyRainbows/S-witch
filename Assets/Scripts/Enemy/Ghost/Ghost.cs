using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : Enemy
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The Unit which the ghost shall follow and attack")]
    protected GameObject theUnit;

    [SerializeField]
    [Tooltip("The Ghost Manager keeping track of the ghost")]
    protected GameObject theManager;

    [SerializeField]
    [Tooltip("How fast the ghost moves when angry")]
    protected float angrySpeed;

    [SerializeField]
    [Tooltip("How fast the ghost moves when scared")]
    protected float scaredSpeed;

    [SerializeField]
    [Tooltip("How long the ghost remains scared")]
    protected float scaredTime;

    [SerializeField]
    [Tooltip("total health of this ghost")]
    protected float ghostHealth;

    [SerializeField]
    [Tooltip("damage ghost does to the player")]
    public float damage;
    #endregion

    #region Private Variables
    protected bool isAngry;
    protected Transform ghostTransform;
    protected Rigidbody2D ghostRB;
    protected Transform unitTransform;
    //protected Color currentColor = GameInformation.nullColor;
    protected Collider[] otherGhosts;
    protected float radius = 1.5f;
    protected Vector3 scaredVelocity;
    protected string clipBool;
    #endregion


    #region Initialization
    protected override void Start()
    {
        isAngry = true;
        ghostTransform = transform;
        ghostRB = GetComponent<Rigidbody2D>();
        unitTransform = theUnit.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        currentColor = GameInformation.nullColor;
        deathClip = "ghost_split";
        clipBool = "split";
        StartCoroutine(StartFight());

        GameManager.instance.SetBossColor(currentColor);
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        if (isAngry && theUnit.GetComponent<UnitBehavior>().CompletedPerfSwitch())
        {
            isAngry = false;
            animator.SetBool("scared", true);
            StartCoroutine(scaredMovement());
        }
        else if (isAngry)
        {
            if(renderer.color != GameInformation.nullColor)
            {
                setColor(GameInformation.nullColor);
            }
            Move();
        }
    }
    #endregion

    #region Damage 
    public override void TakeDamage(float damage)
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }

        currentHealth = theManager.GetComponent<GhostManager>().getCurrentHealth();
        theManager.GetComponent<GhostManager>().lowerSlider(damage);
        healthSlider.value = currentHealth / totalHealth;
        ghostHealth -= damage;

        if (!takingDamage)
        {
            StartCoroutine(TakeDamage());
        }

        if (ghostHealth <= 0)
        {
            Die();
        }
    }

    protected override IEnumerator TakeDamage()
    {
        Color color = currentColor;

        takingDamage = true;

        float flashDelay = .1f;
        for (int i = 0; i < 3; i++)
        {
            renderer.color = Color.red;
            yield return new WaitForSeconds(flashDelay);

            renderer.color = GameManager.instance.GetBossColor();
            yield return new WaitForSeconds(flashDelay);
        }
        setColor(color);
        takingDamage = false;
    }

    public float GhostHealth()
    {
        return ghostHealth;
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameInformation.playerOneTag) 
        || collision.gameObject.CompareTag(GameInformation.playerTwoTag))
        {
            theUnit.GetComponent<UnitBehavior>().TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag))
        {
            if (!isAngry && collision.gameObject.GetComponent<Bullet>().IsColor(currentColor))
            {
                TakeDamage(collision.GetComponent<PlayerBullet>().damage);
            }
        }
        if (collision.gameObject.CompareTag("Wall") && !isAngry)
        {
            scaredVelocity.x *= -1;
        }
        else if (collision.gameObject.CompareTag("HorizontalWall") && !isAngry)
        {
            scaredVelocity.y *= -1;
        }
    }
    #endregion

    #region Movement 
    protected virtual void Move()
    {
        if (Vector3.Distance(transform.position, unitTransform.position) > 1f)
        {
            Vector3 dir = unitTransform.position - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }
    }

    IEnumerator scaredMovement()
    {
        setRandomColor();
        scaredVelocity = (ghostTransform.position - unitTransform.position).normalized;
        float ElapsedTime = 0.0f;
        while (ElapsedTime < scaredTime)
        {
            if (ghostHealth <= 0)
            {
                scaredVelocity = new Vector3(0,0,0);
            }
            ghostTransform.Translate(scaredVelocity * scaredSpeed * Time.deltaTime);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        isAngry = true;
        animator.SetBool("scared", false);
        setColor(GameInformation.nullColor);
    }
    #endregion

    #region Setup Functions
    public void setUnit(GameObject unit)
    {
        theUnit = unit;
    }
    public void setManager(GameObject manager)
    {
        theManager = manager;
    }

    public void setTotalHealth(float val)
    {
        totalHealth = val;
    }
    public void setCurrentHealth(float val)
    {
        currentHealth = val;
    }
    public void setSlider(Slider slider)
    {
        healthSlider = slider;
    }

    public void setColor(Color color)
    {
        currentColor = color;
        renderer.color = color;
    }
    public void setRandomColor()
    {
        float rand = Random.Range(0.0f, 1.0f);
        if (rand > 0.5)
        {
            setColor(GameInformation.pink);
        }
        else
        {
            setColor(GameInformation.purple);
        }
    }
    #endregion

    #region Death Functions
    protected override IEnumerator DeathAnimation()
    {
        animator.SetBool(clipBool, true);
        GetComponent<Collider2D>().enabled = false;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(deathClip))
            {
                yield return new WaitForSeconds(clip.length);
            }
        }
        nextGhosts();
    }

    protected virtual void nextGhosts()
    {
        theManager.GetComponent<GhostManager>().splitGhost(ghostTransform.position);
        Destroy(gameObject);
    }
    #endregion

    protected override void PlayShoot()
    {
        //audio.clip = shoot;
        //audio.Play();
    }

    protected override void PlayHurt()
    {
        //audio.clip = hurt;
        //audio.Play();
    }

    protected override void PlayDie()
    {
        //audio.clip = die;
        //audio.Play();
    }
}
