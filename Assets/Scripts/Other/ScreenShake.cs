using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
    private Transform camTransform;

    private float shakeAmount = 0.7f;
    private Vector3 originalPos;

    void Awake() {
        if (camTransform == null) {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable() {
        originalPos = camTransform.localPosition;
    }

    public void Shake() {
        StartCoroutine(ShakeScreen());
    }

    private IEnumerator ShakeScreen() {
        while (true) {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            yield return new WaitForSeconds(.04f);
        }
    }
}
