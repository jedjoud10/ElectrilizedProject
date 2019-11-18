using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUIScript : MonoBehaviour
{
    public GameObject panel;
    private PlayerController PlayerController;
    private InventoryScript inv;
    public ItemSlotHandler[] slots;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }
    public void updateInventory(InventoryScript inventory)
    {
        inv = inventory;
        UpdateInternalGUI();
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerController != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                panel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (PlayerController != null)
                {
                    PlayerController.GetComponent<InventoryScript>().interactingWithMachine = false;
                    PlayerController.canMove = true;
                    PlayerController.GetComponent<PlayerRaycasterHandler>().currentMachine = null;
                }
            }
        }
    }
    public void SetPanelActive(PlayerController player)
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.canMove = false;
        PlayerController = player;
        PlayerController.GetComponent<InventoryScript>().interactingWithMachine = true;
        inv = player.GetComponent<InventoryScript>();
        UpdateInternalGUI();
    }
    public void UpdateInternalGUI()
    {
        for (int i = 0; i < inv.Items.Length; i++)
        {
            if (inv.Items[i] != null)
            {
                slots[i].SetItem(inv.Items[i]);
            }
            else
            {
                slots[i].SetNothing();
            }
        }
    }
    public void SelectItem(ItemAssetScript item)
    {
        if (GetComponent<SmelterScript>() != null)
        {
            GetComponent<SmelterScript>().setItem(item);
        }
        if (GetComponent<SolderingStationScript>() != null)
        {
            GetComponent<SolderingStationScript>().setItem(item);
        }
    }
    public void UseItem(ItemAssetScript item)//Remove item from player
    {
        inv.RemoveItem(item);
    }
    public bool GiveItem(ItemAssetScript item)//Add item to player
    {
        return inv.AddItem(item);
    }
    public void CollectOutputItem()//Collects the output item
    {
        if (GetComponent<SmelterScript>() != null)
        {
            GetComponent<SmelterScript>().collectOutput();
        }
        if (GetComponent<SolderingStationScript>() != null)
        {
            GetComponent<SolderingStationScript>().collectOutput();
        }
    }
}
