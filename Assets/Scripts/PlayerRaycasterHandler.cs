using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycasterHandler : MonoBehaviour
{
    public float MaxDistance = 5;
    private RaycastHit hit;
    public GameObject cam;
    public MachineUIScript currentMachine;
    private InventoryScript Inv;
    // Start is called before the first frame update
    void Start()
    {
        Inv = GetComponent<InventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                if (hit.distance > MaxDistance)
                {
                    return;
                }
                if (hit.collider.GetComponent<ItemScript>() != null)
                {
                    if (Inv.AddItem(hit.collider.GetComponent<ItemScript>().MyItem))
                    {
                        Destroy(hit.collider.gameObject);
                    }                    
                }
                if (hit.collider.GetComponent<MachineUIScript>() != null)
                {
                    currentMachine = hit.collider.GetComponent<MachineUIScript>();
                    hit.collider.GetComponent<MachineUIScript>().SetPanelActive(GetComponent<PlayerController>());                    
                }
                else
                {
                    currentMachine = null;
                }
            }
        }
    }
}
