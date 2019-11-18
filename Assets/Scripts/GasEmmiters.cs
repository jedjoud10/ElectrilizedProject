using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEmmiters : MonoBehaviour
{
    private float beforevar;
    public float OxygenRemoval;
    public PlayerMechanics pm;
    private float time;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMechanics>() != null)
        {
            beforevar = other.gameObject.GetComponent<PlayerMechanics>().GasEmmiter;
            other.gameObject.GetComponent<PlayerMechanics>().GasEmmiter += OxygenRemoval;
            pm = other.gameObject.GetComponent<PlayerMechanics>();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMechanics>() != null)
        {
            other.gameObject.GetComponent<PlayerMechanics>().GasEmmiter = beforevar;
            pm = null;
        }
    }
    public void Update()
    {
        if (pm != null)
        {
            pm.GasEmmiter += OxygenRemoval * Time.deltaTime;
            time += Time.deltaTime;
        }
    }
}
