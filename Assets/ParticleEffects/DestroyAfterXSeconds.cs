using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour
{
    public float timeToDestroy;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < timeToDestroy)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
