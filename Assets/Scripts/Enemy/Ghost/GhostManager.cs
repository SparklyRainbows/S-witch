using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Unit which the ghost shall follow and attack")]
    protected GameObject theUnit;

    [SerializeField]
    [Tooltip("The ghost prefab that will spawn upon death")]
    protected GameObject ghost;

    [SerializeField]
    [Tooltip("location where the initial ghost will instantiate")]
    protected Vector3 ghostLoc;

    [SerializeField]
    [Tooltip("The smaller ghost prefab that the ghost will spawn upon death")]
    protected GameObject smallerGhost;

    [SerializeField]
    [Tooltip("locations where the smaller ghosts will instantiate")]
    protected Vector3[] smallGhostOffset;

    [SerializeField]
    [Tooltip("The smaller ghost prefab that the ghost will spawn upon death")]
    protected GameObject smallestGhost;

    [SerializeField]
    [Tooltip("locations where the smaller ghosts will instantiate")]
    protected Vector3[] smallestGhostOffset;

    private float totalHealth;
    private float currentHealth;
    private int numGhostsInPlay;
    protected Slider healthSlider;
    protected int smallGhostNum;
    protected int smallestGhostNum;

    // Start is called before the first frame update
    void Start()
    {
        totalHealth = ghost.GetComponent<Ghost>().GhostHealth() 
                        + 2 * smallerGhost.GetComponent<Ghost>().GhostHealth() 
                        + 4 * smallestGhost.GetComponent<Ghost>().GhostHealth();
        currentHealth = totalHealth;
        healthSlider = GameObject.Find("EnemyHealthBar").GetComponent<Slider>();
        numGhostsInPlay = 1;
        smallGhostNum = 0;
        smallestGhostNum = 0;
        spawnGhost(ghostLoc);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (numGhostsInPlay == 0)
        {
            GameManager.instance.Win();
            SceneManagement.Win();
        }*/
    }

    public void spawnGhost(Vector3 loc)
    {
        GameObject sg = Instantiate(ghost, loc, Quaternion.identity);
        sg.GetComponent<Ghost>().setUnit(theUnit);
        sg.GetComponent<Ghost>().setManager(this.gameObject);
        sg.GetComponent<Ghost>().setTotalHealth(totalHealth);
        sg.GetComponent<Ghost>().setCurrentHealth(currentHealth);
        sg.GetComponent<Ghost>().setSlider(healthSlider);
    }

    public void splitGhost(Vector3 loc)
    {
        foreach (Vector3 offset in smallGhostOffset)
        {
            GameObject sg = Instantiate(smallerGhost, loc + offset, Quaternion.identity);
            sg.GetComponent<SmallerGhost>().setGhostNum(smallGhostNum);
            sg.GetComponent<SmallerGhost>().setUnit(theUnit);
            sg.GetComponent<SmallerGhost>().setManager(this.gameObject);
            sg.GetComponent<SmallerGhost>().setTotalHealth(totalHealth);
            sg.GetComponent<SmallerGhost>().setCurrentHealth(currentHealth);
            sg.GetComponent<SmallerGhost>().setSlider(healthSlider);
            smallGhostNum += 1;
        }
        numGhostsInPlay += 1;
    }

    public void splitSmallerGhost(Vector3 loc)
    {
        foreach (Vector3 offset in smallestGhostOffset)
        {
            GameObject sg = Instantiate(smallestGhost, loc + offset, Quaternion.identity);
            sg.GetComponent<SmallestGhost>().setGhostNum(smallestGhostNum);
            sg.GetComponent<SmallestGhost>().setUnit(theUnit);
            sg.GetComponent<SmallestGhost>().setManager(this.gameObject);
            sg.GetComponent<SmallestGhost>().setTotalHealth(totalHealth);
            sg.GetComponent<SmallestGhost>().setCurrentHealth(currentHealth);
            sg.GetComponent<SmallestGhost>().setSlider(healthSlider);
            smallestGhostNum += 1;
        }
        numGhostsInPlay += 1;
    }
    public void lowerSlider(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth / totalHealth;
    }

    public void lowerGhostCount()
    {
        numGhostsInPlay -= 1;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public bool LastGhost() {
        return numGhostsInPlay <= 1;
    }
}
