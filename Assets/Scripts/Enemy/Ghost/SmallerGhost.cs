using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallerGhost : Ghost
{
    protected int ghostNum;
    protected int multiplier;

    protected override void Start()
    {
        isAngry = true;
        ghostTransform = transform;
        ghostRB = GetComponent<Rigidbody2D>();
        unitTransform = theUnit.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        deathClip = "ghost_split";
        clipBool = "split";
    }

    protected override void Move()
    {
        if (Vector3.Distance(unitTransform.position, ghostTransform.position) < 4)
        {
            Vector3 dir = unitTransform.position - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }
        else
        {
            if (ghostNum == 0)
            {
                Vector3 dir = unitTransform.position + new Vector3(4, 0, 0) - ghostTransform.position;
                transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
            }
            else
            {
                Vector3 dir = unitTransform.position + new Vector3(-4, 0, 0) - ghostTransform.position;
                transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
            }
        }
    }

    protected void Circle()
    {
        var rotation = ghostTransform.rotation;
        transform.RotateAround(unitTransform.position, Vector3.forward, 120 * Time.deltaTime);
        transform.rotation = rotation;
    }

    public void setGhostNum(int num)
    {
        ghostNum = num;
    }


    protected override void nextGhosts()
    {
        theManager.GetComponent<GhostManager>().splitSmallerGhost(ghostTransform.position);
        Destroy(gameObject);
    }
}
