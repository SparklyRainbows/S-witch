using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Enemy
{
    [SerializeField]
    [Tooltip("list of positions the cat may stretch to")]
    private Vector3[] stretchPos;

    [SerializeField]
    [Tooltip("offset from cat from which the ball spawns")]
    private Vector3 ballOffset;

    [SerializeField]
    [Tooltip("how fast the cat moves between positions")]
    private float stretchTime;

    [SerializeField]
    [Tooltip("the player object which the cat may follow")]
    private GameObject theUnit;

    [SerializeField]
    [Tooltip("the cat's hat")]
    private GameObject catHat;

    [SerializeField]
    [Tooltip("the cat's health")]
    private float maxHealth;

    [SerializeField]
    [Tooltip("how fast balls speed up by")]
    private float speedup1;

    [SerializeField]
    [Tooltip("how fast balls speed up by")]
    private float speedup2;

    [SerializeField]
    [Tooltip("how long cat wits before moving again")]
    private float attackCooldown;

    private bool moving;
    private int currentLoc;
    private int nextLoc;
    private int phase;
    private bool hatUp;

    //Color currentHatColor;

    protected override void Start()
    {
        totalHealth = maxHealth;
        moving = false;
        hatUp = false;
        currentColor = catHat.GetComponent<CatHat>().getColor();
        currentLoc = 0;
        phase = 1;

        GameManager.instance.SetBossColor(GameInformation.nullColor);

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            if (currentHealth < maxHealth * 13 / 16 && phase < 2)
            {
                phase = 2;
            }
            if (currentHealth < maxHealth * 10 / 16 && phase < 3)
            {
                phase = 3;
                stretchTime = stretchTime / speedup1;
            }
            if (currentHealth < maxHealth * 7 / 16 && phase < 4)
            {
                phase = 4;
            }
            if (currentHealth < maxHealth * 1 / 4 && phase < 5)
            {
                phase = 5;
                stretchTime = stretchTime / speedup2;
            }

            StartCoroutine(Stretch(randomStretchPos()));
        }

    }

    private IEnumerator Stretch(Vector3 nextPos)
    {
        if (currentLoc != nextLoc)
        {
            animator.SetBool("attack", false);
            if(hatUp)
            {
                catHat.transform.position += Vector3.down;
                hatUp = false;
            }
        }
        moving = true;
        float elapsedTime = 0.0f;
        Color elapsedColor = currentColor;
        Color nextColor = GetNextColor();
        while (elapsedTime < stretchTime)
        {
            elapsedTime += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, nextPos, elapsedTime / stretchTime);
            elapsedColor = Color.Lerp(catHat.GetComponent<CatHat>().getColor(), nextColor, elapsedTime / stretchTime);
            catHat.GetComponent<CatHat>().setSpriteColor(elapsedColor);
            yield return null;
        }
        currentLoc = nextLoc;
        catHat.GetComponent<CatHat>().setColor(nextColor);
        currentColor = nextColor;
        animator.SetBool("attack", true);
        if (!hatUp)
        {
            catHat.transform.position += Vector3.up;
            hatUp = true;
        }
        Upchuck(nextColor);

        yield return new WaitForSeconds(attackCooldown);

        moving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag))
        {
            if (currentColor == GameInformation.nullColor ||
                collision.gameObject.GetComponent<Bullet>().IsColor(currentColor))
            {
                TakeDamage(collision.GetComponent<PlayerBullet>().damage);
            }
        }
    }

    private Vector3 randomStretchPos()
    {
        nextLoc = Random.Range(0, 3);
        return stretchPos[nextLoc];
    }

    private Vector3 playerStretchPos()
    {
        Vector3 unitPos = theUnit.transform.position;
        if (unitPos.y > 1.7)
        {
            nextLoc = 2;
            return stretchPos[2];
        }
        else if (unitPos.y < -1.7)
        {
            nextLoc = 0;
            return stretchPos[0];
        }
        else
        {
            nextLoc = 1;
            return stretchPos[1];
        }

    }

    private Color GetNextColor()
    {
        if (phase == 1 || phase == 3)
        {
            return GameInformation.pink;

        }
        else if (phase == 2 || phase == 4) {
            return GameInformation.purple;
        }
        else
        {
            int ballColor = Random.Range(0, 2);
            if (ballColor == 0)
            {
                return GameInformation.purple;
            }
            else
            {
                return GameInformation.pink;
            }
        }
    }

    private void Upchuck(Color color)
    {
        PlayShoot();
        GameObject Hairball = Instantiate(bulletObj, stretchPos[currentLoc] + ballOffset, Quaternion.identity);
        Hairball.GetComponent<EnemyBullet>().SetColor(color);
    }
}
