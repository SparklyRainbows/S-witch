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

    #region Private Variables
    //true if the witch is the inner part of the unit
    public bool inside;

    //true if witch is trying to switch
    private bool readyToSwitch;

    private bool readyToUlt;

    private float myTime;

    private UnitBehavior unit;

    private Rigidbody2D unitRigidbody;

    private bool rotating = false;

    private Animator animator;
    #endregion 

    #region Initialization
    // Start is called before the first frame update
    private void Awake()
    {
        myTime = fireRate;
        readyToSwitch = false;
        readyToUlt = false;
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
        if (getUltButton() && !readyToUlt)
        {
            readyToUlt = true;
            StartCoroutine(ultTimer());
        }
    }
    #endregion


    #region Outer Movement
    public void SetAim(Vector2 aim)
    {
        // set position of child 
        otherWitch.transform.localPosition = aim.normalized * witchDistance;
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.IsGameOver())
        {
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
    }

    //Check if the witch is currently trying to switch
    public bool ReadyToSwitch()
    {
        return readyToSwitch;
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

    public bool ReadyToUlt()
    {
        return readyToUlt;
    }

    IEnumerator ultTimer()
    {
        float ElapsedTime = 0f;
        float TotalTime = 1f;
        while (ElapsedTime < TotalTime)
        {
            if (readyToUlt == false)
            {
                yield break;
            }
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        readyToUlt = false;
    }

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
            return Input.GetButtonDown("Switch");
        }
        else
        {
            return Input.GetButtonDown("Switch2");
        }
    }
    #endregion 

}
