using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallestGhost : SmallerGhost
{
    private bool dead;

    protected override void Start()
    {
        isAngry = true;
        ghostTransform = transform;
        ghostRB = GetComponent<Rigidbody2D>();
        unitTransform = theUnit.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        deathClip = "ghost_death";
        clipBool = "dead";
    }

    protected override void Move()
    {
        if (dead) {
            return;
        }

        if (Vector3.Distance(unitTransform.position, ghostTransform.position) < 3)
        {
            Vector3 dir = unitTransform.position - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }

        else if(ghostNum == 0)
        {
            Vector3 dir = unitTransform.position + new Vector3(0, 3, 0) - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }

        else if (ghostNum == 1)
        {
            Vector3 dir = unitTransform.position + new Vector3(0, -3, 0) - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }
        else if (ghostNum == 2)
        {
            Vector3 dir = unitTransform.position + new Vector3(3, 0, 0) - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }
        else
        {
            Vector3 dir = unitTransform.position + new Vector3(-3, 0, 0) - ghostTransform.position;
            transform.Translate(dir.normalized * angrySpeed * Time.deltaTime);
        }
    }

    protected override IEnumerator DeathAnimation() {
        if (theManager.GetComponent<GhostManager>().LastGhost()) {
            GetComponent<Collider2D>().enabled = false;
            GameManager.instance.Win();

            dead = true;

            GameObject.Find("Main Camera").GetComponent<ScreenShake>().Shake();
            StartCoroutine(SpawnDeathParticles());
            yield return new WaitForSeconds(2.5f);
            yield return FadeToBlack();

            Destroy(gameObject);
            SceneManagement.Win();

        } else {

            animator.SetBool(clipBool, true);
            GetComponent<Collider2D>().enabled = false;

            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips) {
                if (clip.name.Equals(deathClip)) {
                    yield return new WaitForSeconds(clip.length);
                }
            }
            nextGhosts();
        }
    }

    protected override void nextGhosts()
    {
        Destroy(gameObject);
        theManager.GetComponent<GhostManager>().lowerGhostCount();
    }
}
