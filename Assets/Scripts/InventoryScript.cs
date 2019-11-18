using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryScript : MonoBehaviour
{
    public ItemAssetScript[] Items;
    public int MaxItems;
    public GameObject panel;
    public bool interactingWithMachine = false;
    public ItemSlotHandler[] slots;
    private PlayerRaycasterHandler raycastHandler;
    public Text text;
    public GameObject buildButton;
    public Camera3DItemScript item3DHandler;
    // Start is called before the first frame update
    void Start()
    {
        if (Items.Length != MaxItems)
        {
            Items = new ItemAssetScript[4];
        }
        panel.SetActive(false);
        raycastHandler = GetComponent<PlayerRaycasterHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactingWithMachine)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                panel.SetActive(!panel.activeSelf);
                GetComponent<PlayerController>().canMove = !panel.activeSelf;
                Cursor.visible = panel.activeSelf;
                if (panel.activeSelf)
                {
                    updateGUI();
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
    public bool AddItem(ItemAssetScript new_item)//add item to inventory
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = new_item;
                updateGUI();
                return true;
            }
        }
        return false;
    }
    public void RemoveItem(ItemAssetScript old_item)//remove item from inventory
    {
        bool removed = false;
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == old_item && !removed)
            {
                Items[i] = null;
                removed = true;
            }
        }
        text.text = "";
        item3DHandler.deselect();
        buildButton.SetActive(false);
        updateGUI();
    }
    public void updateGUI()// update inventory gui
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null)
            {
                slots[i].SetItem(Items[i]);
            }
            else
            {
                slots[i].SetNothing();
            }
        }
        if (raycastHandler.currentMachine != null)
        {
            raycastHandler.currentMachine.updateInventory(this);
        }
        text.text = "";
        item3DHandler.deselect();
        buildButton.SetActive(false);        
    }
    public void selectItem(ItemAssetScript item)//select item
    {
        if (item != null)
        {
            text.text = item.description;
            item3DHandler.selectItem(item);           
            buildButton.SetActive(item.Item_Type == ItemAssetScript.Item_Type_Enum.Machine);
            if (item.Item_Type == ItemAssetScript.Item_Type_Enum.Machine)
            {
                GetComponent<PlayerMachineBuilder>().selectMachine(item);
            }
        }
    }
}
