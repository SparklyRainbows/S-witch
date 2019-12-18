using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Thing that heals you")]
    private GameObject healthPack;
    private float spawnDelay = 10f;

    private void Start() {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        while (true) {
            yield return new WaitForSeconds(spawnDelay);

            Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1f, Random.Range(.1f, .9f), 1));
            Instantiate(healthPack, pos, Quaternion.identity);
        }
    }
}
