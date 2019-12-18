using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [SerializeField]
    [Tooltip("how fast the forcefield turns on and off")]
    private float colorChangeTime;

    public ParticleSystem electrcParticles;

    private ParticleSystem electricity;
    private Color originalColor;
    private Color currentColor;
    private SpriteRenderer spriteRenderer;
    private bool on;


    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        currentColor = originalColor;
        electricity = Instantiate(electrcParticles, transform.position, Quaternion.identity);
        on = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameInformation.playerSpellTag) && on)
        {
            Destroy(collision.gameObject);
            Instantiate(collision.GetComponent<PlayerBullet>().lilBurst, collision.transform.position, Quaternion.identity);
        }
    }

    public void TurnOff()
    {
        StartCoroutine(ChangeColor(GameInformation.nullColor));
        Destroy(electricity);
        on = false;
    }

    public void TurnOn()
    {
        StartCoroutine(ChangeColor(originalColor));
        electricity = Instantiate(electrcParticles, transform.position, Quaternion.identity);
        on = true;
    }

    private IEnumerator ChangeColor(Color nextColor)
    {
        float elapsedTime = 0.0f;
        Color elapsedColor = currentColor;
        while (elapsedTime < colorChangeTime)
        {
            elapsedTime += Time.deltaTime;
            elapsedColor = Color.Lerp(elapsedColor, nextColor, elapsedTime / colorChangeTime);
            spriteRenderer.color = elapsedColor;
            yield return null;
        }
        currentColor = nextColor;
    }
}
