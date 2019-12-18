using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public int playerNum;
    public float spacing;
    public float moveTime;
    public float speedMultiplier;
    public ParticleSystem readySparkles;

    //private float currentTime;

    //public GameObject topRight;
    //public GameObject topMid;
    //public GameObject topLeft;
    //public GameObject midRight;
    //public GameObject midLeft;
    //public GameObject bottomRight;
    //public GameObject bottomMid;
    //public GameObject bottomLeft;

    //Vector3 topRightLoc;
    //Vector3 topMidLoc;
    //Vector3 topLeftLoc;
    //Vector3 midRightLoc;
    //Vector3 midLeftLoc;
    //Vector3 bottomRightLoc;
    //Vector3 bottomMidLoc;
    //Vector3 bottomLeftLoc;

    //Vector3 currentLoc;

    //private GameObject[,] bosses;
    //private Vector3[,] bossLocs;
    //private int[] location;
    private bool ready;
    private ParticleSystem sparkles;
    private Rigidbody2D rb;

    private Vector3 screenBounds;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        ready = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
        {
            float hAxis = getAxisH();
            float vAxis = getAxisV();
            Vector2 movement = new Vector2(hAxis, vAxis);
            rb.velocity = movement * speedMultiplier;
        }
    }

    private void LateUpdate() {
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -screenBounds.x, screenBounds.x);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y, screenBounds.y);
        transform.position = pos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelSelect"))
        {
            if (!collision.GetComponent<LevelSelect>().CanSelect()) {
                return;
            }


            if (ready)
            {
                if (getFireButton())
                {
                    collision.GetComponent<LevelSelect>().Unready();
                    ready = false;
                    Destroy(sparkles);
                }
            }

            else
            {
                if (getFireButton())
                {
                    rb.velocity = Vector3.zero;
                    collision.GetComponent<LevelSelect>().Ready();
                    ready = true;
                    sparkles = Instantiate(readySparkles, transform.position, Quaternion.identity);
                }
            }

        }

    }

    private bool getFireButton()
    {
        if (playerNum == 1)
        {
            return Input.GetButtonDown("Fire1");
        }
        else
        {
            return Input.GetButtonDown("Fire2");
        }
    }

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


}
