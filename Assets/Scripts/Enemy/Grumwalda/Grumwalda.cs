using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grumwalda : Enemy
{
    #region attack stage order
    private Queue<Boss> stageOrder;
    private Boss currentStage;

    //If Grumwalda has this percent of health, she'll go into her final phase
    private float finalStagePercent = 0f;
    private float finalStageHealth;
    //Array of floats of the amount of health she'll have before going into the next phase
    private Stack<float> stageChangeHealthInfo;
    #endregion

    #region attack_information

    #region bullet objects
    [Header("Spells")]
    public GameObject hairball;
    public GameObject hand;
    public GameObject ghost;
    public GameObject pumpkinseed;
    public GameObject forcefield;
    public GameObject[] turretObjs;
    public GameObject bone;
    public GameObject boomerangBone;
    public GameObject bat;
    #endregion

    #region pumpkin_attack_vars
    private float pumpkin_attackDelay = .3f;
    private float pumpkin_colorSwitchTimer = 5f;
    #endregion
    #region skeleton_attack_vars
    private float skeleton_attackDelay = .7f;
    private float skeleton_colorSwitchTimer = 7f;
    #endregion
    #region vampire_attack_vars
    private float vampire_attackDelay = .8f;
    private float vampire_colorSwitchTimer = 5f;
    #endregion
    #region frankenstein_attack_vars
    private float frankenstein_attackDelay = 2.4f;
    private float frankenstein_colorSwitchTimer = 5f;
    #endregion
    #region cat_attack_vars
    private float cat_attackDelay = .5f;
    private float cat_colorSwitchTimer = 5f;
    private Vector2[] hairball_locations;
    #endregion
    #region ghost_attack_vars
    private int numOfGhosts = 0;
    private float ghost_attackDelay = 3f;
    private float ghost_colorSwitchTimer = 3f;
    #endregion
    #region scientist_attack_vars
    public GameObject pinkLaser;
    public GameObject purpleLaser;
    private Turret[] turrets;
    private GameObject forcefieldObj;
    private bool foughtScientist;
    #endregion

    #endregion

    #region start funcs
    protected override void Start() {
        totalHealth = 300f;
        finalStageHealth = totalHealth * finalStagePercent;

        currentColor = GameInformation.purple;

        if (GameInformation.defeatedBosses.Count < 7) {
            SetStageOrder();
        }
        InitStageHealthInfo();

        stageOrder = new Queue<Boss>(GameInformation.defeatedBosses);
        currentStage = stageOrder.Dequeue();

        //Set up cat hairball info
        hairball_locations = new Vector2[]{
            new Vector2(6f, 2.5f),
            new Vector2(6f, 0f),
            new Vector2(6f, -2.5f)
        };

        //Disable turrets
        foreach (GameObject t in turretObjs) {
            t.SetActive(false);
        }

        StartCoroutine(SwitchColor());
        base.Start();
    }

    private void SetStageOrder() {
        GameInformation.defeatedBosses = new List<Boss>(){
            Boss.SCIENTIST,
            Boss.CAT,
            Boss.FRANKENSTEIN,
            Boss.GHOST,
            Boss.PUMPKIN,
            Boss.SKELETON,
            Boss.VAMPIRE
        };
    }

    private void InitStageHealthInfo() {
        stageChangeHealthInfo = new Stack<float>();

        stageChangeHealthInfo.Push(finalStageHealth);
        float stageHealth = (totalHealth - finalStageHealth)/GameInformation.defeatedBosses.Count;
        for (int i = 0; i < GameInformation.defeatedBosses.Count - 1; i++) {
            float prevHealth = stageChangeHealthInfo.Peek();
            stageChangeHealthInfo.Push(prevHealth + stageHealth);
        }
    }

    #endregion

    #region switch color funcs
    private IEnumerator SwitchColor() {
        while (true) {
            if (currentStage == Boss.SCIENTIST) {
                currentColor = GameInformation.nullColor;
            } else {
                currentColor = currentColor == GameInformation.purple ? GameInformation.pink : GameInformation.purple;
            }

            GetComponent<SpriteRenderer>().color = currentColor;
            GameManager.instance.SetBossColor(currentColor);

            yield return new WaitForSeconds(GetColorSwitchTimer());
        }
    }

    private float GetColorSwitchTimer() {
        switch(currentStage) {
            case Boss.PUMPKIN:
                return pumpkin_colorSwitchTimer;
            case Boss.SKELETON:
                return skeleton_colorSwitchTimer;
            case Boss.VAMPIRE:
                return vampire_colorSwitchTimer;
            case Boss.FRANKENSTEIN:
                return frankenstein_colorSwitchTimer;
            case Boss.CAT:
                return cat_colorSwitchTimer;
            case Boss.GHOST:
                return ghost_colorSwitchTimer;
            case Boss.SCIENTIST:
                return 2f;
            default:
                Debug.LogWarning($"Grumwalda attack stage not found: {currentStage}");
                return .1f;
        }
    }
    #endregion

    #region attack_funcs
    protected override IEnumerator Attack() {
        while (true) {
            if (GameManager.instance.IsGameOver()) {
                break;
            }

            PlayShoot();
            yield return ChooseAttack();
        }
    }

    private IEnumerator ChooseAttack() {
        switch(currentStage) {
            case Boss.PUMPKIN:
                yield return PumpkinAttack();
                break;
            case Boss.SKELETON:
                yield return SkeletonAttack();
                break;
            case Boss.VAMPIRE:
                yield return VampireAttack();
                break;
            case Boss.FRANKENSTEIN:
                yield return FrankensteinAttack();
                break;
            case Boss.CAT:
                yield return CatAttack();
                break;
            case Boss.GHOST:
                yield return GhostAttack();
                break;
            case Boss.SCIENTIST:
                if (!foughtScientist) {
                    yield return ScientistAttack();
                } else {
                    yield return new WaitForSeconds(.1f);
                }
                break;
            default:
                Debug.Log($"Grumwalda attack stage not found for {currentStage}");
                break;
        }
    }
    #endregion

    #region BossAttacks
    private GameObject ShootBullet(GameObject bulletObj) {
        GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        return bullet;
    }

    private IEnumerator PumpkinAttack() {
        GameObject bullet = ShootBullet(pumpkinseed);
        bullet.GetComponent<EnemyBullet>().SetColor(currentColor);

        yield return new WaitForSeconds(pumpkin_attackDelay);
    }
    
    private IEnumerator SkeletonAttack() {
        float random = Random.Range(0, 2);
        if (random == 0) {
            ShootBullet(boomerangBone);
        } else {
            GameObject bullet = ShootBullet(bone);
            bullet.GetComponent<SpinningBullet>().SetTarget(Random.Range(120, 250));
        }

        yield return new WaitForSeconds(skeleton_attackDelay);
    }

    private IEnumerator VampireAttack() {
        ShootBullet(bat);

        yield return new WaitForSeconds(vampire_attackDelay);
    }

    private IEnumerator FrankensteinAttack() {
        GameObject bullet = ShootBullet(hand);
        bullet.GetComponent<Hand>().SetTarget(Random.Range(120, 250));

        yield return new WaitForSeconds(frankenstein_attackDelay);
    }

    private IEnumerator CatAttack() {
        Vector3 location = hairball_locations[Random.Range(0, hairball_locations.Length)];
        GameObject bullet = ShootBullet(hairball);
        bullet.transform.position = location;

        float random = Random.Range(0, 2);
        if (random == 0) {
            bullet.GetComponent<Hairball>().SetColor(GameInformation.purple);
        } else {
            bullet.GetComponent<Hairball>().SetColor(GameInformation.pink);
        }

        yield return new WaitForSeconds(cat_attackDelay);
    }

    private IEnumerator GhostAttack() {
        yield return null;
        if (numOfGhosts < 3) {
            numOfGhosts++;

            GameObject bullet = ShootBullet(ghost);

            yield return new WaitForSeconds(ghost_attackDelay);
        }
    }

    private IEnumerator ScientistAttack() {
        forcefieldObj = ShootBullet(forcefield);
        forcefieldObj.transform.position = new Vector2(forcefieldObj.transform.position.x - 2, forcefieldObj.transform.position.y);

        foreach (GameObject t in turretObjs) {
            t.SetActive(true);
        }

        turrets = new Turret[turretObjs.Length];
        for (int i = 0; i < turretObjs.Length; i++) {
            turrets[i] = turretObjs[i].GetComponent<Turret>();
        }

        //Set turret colors
        for (int i = 0; i < turrets.Length; i++) {
            if (i % 2 == 0) {
                turrets[i].SetColor(GameInformation.pink);
                turrets[i].SetBullet(pinkLaser);
            } else {
                turrets[i].SetColor(GameInformation.purple);
                turrets[i].SetBullet(purpleLaser);
            }
        }

        foreach (Turret t in turrets) {
            StartCoroutine(t.Shooter());
        }

        while (!(turrets[0].IsDead() && turrets[1].IsDead()
            && turrets[2].IsDead() && turrets[3].IsDead())) {
            yield return null;
        }

        Destroy(forcefieldObj);
        Destroy(GameObject.Find("Electricity(Clone)"));
        foreach(GameObject t in turretObjs) {
            Destroy(t);
        }
        foughtScientist = true;
    }
    #endregion

    #region Damage funcs
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag)) {
            if (currentColor == GameInformation.nullColor ||
                collision.gameObject.GetComponent<Bullet>().IsColor(currentColor)) {
                TakeDamage(collision.GetComponent<PlayerBullet>().damage);
            }
        }
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);

        if (stageChangeHealthInfo.Count == 0) {
            return;
        }

        if (currentHealth <= stageChangeHealthInfo.Peek()) {
            stageChangeHealthInfo.Pop();

            //If we have reached the final stage
            if (stageOrder.Count == 0 || currentHealth <= finalStageHealth) {
                currentStage = Boss.GRUMWALDA;
            } else {
                currentStage = stageOrder.Dequeue();
            }
        }

        //If we just defeated the scientist and the boss has changed, destroy the forcefield
        if (foughtScientist && currentStage != Boss.SCIENTIST && forcefieldObj != null) {
            Destroy(forcefieldObj);
        }
    }

    protected override void Die() {
        GameManager.instance.gameWon = true;
        base.Die();
    }
    #endregion

    public void KilledGhost() {
        numOfGhosts--;
    }
}
