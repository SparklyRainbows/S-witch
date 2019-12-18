using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
    public bool pink;
    public GameObject next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            if (pink && collision.gameObject.GetComponent<PlayerBullet>().IsColor(GameInformation.pink))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
                Destroy(collision.gameObject);
                next.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else if (!pink && !collision.gameObject.GetComponent<PlayerBullet>().IsColor(GameInformation.pink))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
                Destroy(collision.gameObject);
                next.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
