using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWall : MonoBehaviour
{
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
        if (!collision.gameObject.CompareTag("PlayerProjectile"))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            next.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
