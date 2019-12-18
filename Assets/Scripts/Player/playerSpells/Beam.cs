using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{
    [SerializeField]
    [Tooltip("Ult Duration")]
    private float ultTime;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Ult());
    }

    IEnumerator Ult()
    {
        yield return new WaitForSeconds(ultTime);
        Destroy(gameObject);
    }
}
