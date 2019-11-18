using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDirection : MonoBehaviour
{
    public Vector3 speed;
    private PlayerMechanics pm;
    private Light lightsun;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.FindObjectOfType<PlayerMechanics>();
        lightsun = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
        pm.timeofday = Vector3.Dot(transform.forward, Vector3.up) / 2 + 0.5f;
        lightsun.intensity = 1 - (Vector3.Dot(transform.forward, Vector3.up) / 2 + 0.5f) * 0.8f;
    }
}
