using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSlider : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Indicator")]
    private GameObject indicator;

    [SerializeField]
    [Tooltip("Speed")]
    private float rate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        indicator.transform.localPosition = new Vector3(Mathf.PingPong(Time.time * rate, .14f) - .07f, -0.03f, 0);
    }

    public bool PerfectSwitch()
    {
        return indicator.transform.localPosition.x > -.02f && indicator.transform.localPosition.x < .02f;
    }
}
