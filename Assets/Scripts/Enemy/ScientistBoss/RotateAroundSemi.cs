using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSemi : MonoBehaviour
{
    [SerializeField]
    [Tooltip("what the object rotates around")]
    private GameObject turretBase;

    [SerializeField]
    [Tooltip("the speed at which it rotates")]
    private float rotationSpeed;

    private float currentRotationSpeed;

    private bool check;

    private void Start()
    {
        StartRotating();
        check = true;
    }

    void Update()
    {
        if (transform.localPosition.x > 0 && check)
        {
            check = false;
            currentRotationSpeed *= -1;
            StartCoroutine(Checker());
        }
        transform.RotateAround(turretBase.transform.position, Vector3.forward, currentRotationSpeed * Time.deltaTime);
    }

    public void ScaleRotationSpeed(float scale)
    {
        rotationSpeed *= scale;
    }

    public void StopRotating()
    {
        currentRotationSpeed = 0;
        StopAllCoroutines();
    }
    public void StartRotating()
    {
        currentRotationSpeed = rotationSpeed;
    }

    private IEnumerator Checker()
    {
        yield return new WaitForSeconds(0.1f);
        check = true;
    }
}
