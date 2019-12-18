using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    [Tooltip("how fast the player moves")]
    private float speedMultiplier;

    [SerializeField]
    [Tooltip("how fast the player shoots")]
    private float fireRate;

    [SerializeField]
    [Tooltip("Other Witch")]
    private GameObject otherWitch;

    [SerializeField]
    [Tooltip("the number representing which player controls this witch")]
    private int playerNum;

    private Color color;

    public GameObject bullet;

    private float witchDistance;

    private Vector2 lastAim;

    [SerializeField]
    [Tooltip("how fast the outer witch rotates")]
    private float rotateSpeed;

    #region Private Variables
    //true if the witch is the inner part of the unit
    public bool inside;

    //true if witch is trying to switch
    private bool readyToSwitch;

    private bool readyToAttack;

    private bool readyToWave;

    private float myTime;

    private UnitBehavior unit;

    private Rigidbody2D unitRigidbody;

    private bool rotating = false;

    private Animator animator;

    private float switchDownTime;

    private Vector2 targetAim;
    #endregion 

    #region Initialization
    // Start is called before the first frame update
    private void Awake()
    {
        myTime = fireRate;
        readyToSwitch = false;
        readyToAttack = false;
    }
    void Start()
    {
        unitRigidbody = GetComponentInParent<Rigidbody2D>();
        unit = GetComponentInParent<UnitBehavior>();
        animator = GetComponent<Animator>();

        witchDistance = GetComponentInParent<UnitBehavior>().witchDistance;
        lastAim = new Vector2(1, 0);

        if (gameObject.name.Equals("Witch"))
        {
            color = GameInformation.pink;
        }
        else
        {
            color = GameInformation.purple;
        }
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        if (IsInside()) {
            GetComponent<Collider2D>().enabled = true;
        } else {
            GetComponent<Collider2D>().enabled = false;
        }


        animator.SetBool("flying", IsInside());
        float hAxis = getAxisH();
        float vAxis = getAxisV();

        if (IsInside())
        {
            Vector2 movement = new Vector2(hAxis, vAxis);
            unitRigidbody.velocity = movement * speedMultiplier;
        }
        else
        {
            Vector2 aim = new Vector2(hAxis, vAxis);
            if (aim.magnitude > 0.1f)
            {
                lastAim = new Vector2(hAxis, vAxis);
            }
            otherWitch.GetComponent<PlayerController>().SetAim(lastAim);
            if (getFireButton() && myTime >= fireRate)
            {
                unit.PlayShoot();

                animator.SetTrigger("attack");

                myTime = 0.0f;
                GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
                newBullet.GetComponent<PlayerBullet>().SetDirection(transform.position - unit.transform.position);
                newBullet.GetComponent<PlayerBullet>().SetColor(color);
                newBullet.GetComponent<Rigidbody2D>().velocity = transform.position - unit.transform.position;
            }
            myTime += Time.deltaTime;
        }
        if (getSwitchButton() && !readyToSwitch)
        {
            readyToSwitch = true;
            StartCoroutine(switchTimer());
        }

        if (getUltButton() && !readyToAttack)
        {
            readyToAttack = true;
            StartCoroutine(AttackTimer());
        }
        switchDownTime += Time.deltaTime;
    }
    #endregion

    #region Outer Movement
    public void SetAim(Vector2 aim)
    {
        // set position of child 
        targetAim = aim.normalized * witchDistance;
        float angle = Vector3.SignedAngle(targetAim, otherWitch.transform.localPosition, Vector3.forward);
        if (otherWitch.transform.localPosition != new Vector3(targetAim.x, targetAim.y, 0))
            otherWitch.transform.localPosition = Vector3.RotateTowards(otherWitch.transform.localPosition, new Vector3(targetAim.x, targetAim.y, 0), rotateSpeed * Time.deltaTime, 0);
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }

        if (unit.TakingDamage()) {
            return;
        }

        if (collision.gameObject.CompareTag(GameInformation.enemyBulletTag))
        {
            unit.TakeDamage(collision.gameObject.GetComponent<EnemyBullet>().damage);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }

        if (collision.gameObject.CompareTag(GameInformation.enemyBulletTag))
        {
            unit.TakeDamage(collision.gameObject.GetComponent<EnemyBullet>().damage);
        }
    }

    public void TakeDamage(float amount)
    {
        unit.TakeDamage(amount);
    }
    #endregion

    #region Switch Methods
    //Check if the witch is the inner part of the unit
    public bool IsInside()
    {
        return inside;
    }

    //Change 
    public void Switch()
    {
        inside = !inside;
        switchDownTime = 0;
    }

    //Check if the witch is currently trying to switch
    public bool ReadyToSwitch()
    {
        return getSwitchButton() && !(switchDownTime < 1f);
    }

    public void Unready()
    {
        readyToSwitch = false;
    }

    //Coroutine which lasts however many frames we want the witch to be able to switch
    IEnumerator switchTimer()
    {
        float ElapsedTime = 0.0f;
        float TotalTime = 1.0f;
        while (ElapsedTime < TotalTime)
        {
            if (readyToSwitch == false)
            {
                yield break;
            }
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        readyToSwitch = false;
    }
    #endregion

    #region Spell Methods
    public bool ReadyToAttack()
    {
        return readyToAttack;
    }

    public bool ReadyToWave()
    {
        return getWaveButton();
    }

    IEnumerator AttackTimer()
    {
        float ElapsedTime = 0f;
        float TotalTime = 1f;
        while (ElapsedTime < TotalTime)
        {
            if (readyToAttack == false)
            {
                yield break;
            }
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        readyToAttack = false;
    }
    #endregion

    #region Misc
    private float getAxisH()
    {
        if (playerNum == 1)
        {
            return Input.GetAxis("Horizontal");
        }
        else
        {
            return Input.GetAxis("Horizontal2");
        }
    }

    private float getAxisV()
    {
        if (playerNum == 1)
        {
            return Input.GetAxis("Vertical");
        }
        else
        {
            return Input.GetAxis("Vertical2");
        }
    }

    private bool getFireButton()
    {
        if (playerNum == 1)
        {
            return Input.GetButton("Fire1");
        }
        else
        {
            return Input.GetButton("Fire2");
        }
    }

    private bool getUltButton()
    {
        if (playerNum == 1)
        {
            return Input.GetButton("Ult1");
        }
        else
        {
            return Input.GetButton("Ult2");
        }
    }

    private bool getSwitchButton()
    {
        if (playerNum == 1)
        {
            return Input.GetButton("Switch");
        }
        else
        {
            return Input.GetButton("Switch2");
        }
    }

    public bool getWaveButton()
    {
        if (inside)
        {
            if (playerNum == 1)
            {
                return Input.GetButtonDown("Wave1");
            }
            else
            {
                return Input.GetButtonDown("Wave2");
            }
        }
        return false;
    }
    #endregion 

}
