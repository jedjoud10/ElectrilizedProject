using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMachineBuilder : MonoBehaviour
{
    public GameObject positionMachineBuild;
    public float distance;
    public ItemAssetScript machine;
    private RaycastHit hit;
    public float offset;
    public void buildMachine()
    {
        if (Physics.Raycast(positionMachineBuild.transform.position, Vector3.down * distance, out hit))
        {
            if (hit.distance > distance)
            {
                return;
            }
            GameObject machinevar = Instantiate(machine.Machine, hit.point + Vector3.up * offset, Quaternion.LookRotation(gameObject.transform.forward, hit.normal));
            GetComponent<InventoryScript>().RemoveItem(machine);
            if (machinevar.GetComponent<MiniMinerScript>() != null)
            {
                machinevar.GetComponent<MiniMinerScript>().player = gameObject;
            }
        }
    }
    public void selectMachine(ItemAssetScript machineItem)
    {
        if (machineItem != null)
        {
            machine = machineItem;
        }
    }
}
