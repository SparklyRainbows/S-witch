using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : Enemy
{
    [SerializeField]
    [Tooltip("total health of scientist")]
    private float health;

    [SerializeField]
    [Tooltip("turrets used by the scientist")]
    private GameObject[] turrets;

    [SerializeField]
    [Tooltip("how long the scientist stays scared")]
    private float scaredTime;

    [SerializeField]
    [Tooltip("forceField protecting the scientist")]
    private GameObject forceField;

    public GameObject pinkLaser;
    public GameObject purpleLaser;

    private bool scared;
    private bool phase2;
    private bool phase3;

    // Start is called before the first frame update
    protected override void Start()
    {
        currentHealth = health;
        totalHealth = health;
        scared = false;
        SetTurretColors();
        phase2 = false;
        phase3 = false;

        currentColor = GameInformation.nullColor;
        GameManager.instance.SetBossColor(GameInformation.nullColor);

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (turrets[0].GetComponent<Turret>().IsDead() && turrets[1].GetComponent<Turret>().IsDead()
        && turrets[2].GetComponent<Turret>().IsDead() && turrets[3].GetComponent<Turret>().IsDead() && !scared)
        {
            StartCoroutine(Scared());
        }

        if (currentHealth < 2 * totalHealth / 3 && !phase2)
        {
            NextPhase();
            phase2 = true;
        }
        else if (currentHealth < totalHealth / 3 && !phase3)
        {
            NextPhase();
            phase3 = true;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag) && scared)
        {
            TakeDamage(collision.GetComponent<PlayerBullet>().damage);
        }
    }
    private IEnumerator Scared()
    {
        scared = true;
        float elaspedTime = 0.0f;
        forceField.GetComponent<ForceField>().TurnOff();
        while (elaspedTime <= scaredTime)
        {
            //animator.SetBool("scared", true);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        forceField.GetComponent<ForceField>().TurnOn();
        //animator.SetBool("scared", false);
        scared = false;
        Reset();
    }

    private void Reset()
    {
        foreach (GameObject turr in turrets)
        {
            turr.GetComponent<Turret>().ResetTurret();
        }
    }

    private void SetTurretColors()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            if (i % 2 == 0)
            {
                turrets[i].GetComponent<Turret>().SetColor(GameInformation.pink);
                turrets[i].GetComponent<Turret>().SetBullet(pinkLaser);
            }
            else
            {
                turrets[i].GetComponent<Turret>().SetColor(GameInformation.purple);
                turrets[i].GetComponent<Turret>().SetBullet(purpleLaser);
            }
        }
    }
    private void NextPhase()
    {
        foreach (GameObject turr in turrets)
        {
            turr.GetComponent<Turret>().NextPhase();
        }
    }
}