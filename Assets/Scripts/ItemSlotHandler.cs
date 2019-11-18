using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlotHandler : MonoBehaviour
{
    public ItemAssetScript myitem;
    public MachineUIScript handler;
    public InventoryScript inv;
    public Text nametext;

    public void SetItem(ItemAssetScript item)
    {
        nametext.text = item.name.Replace("_"," ");
        myitem = item;
    }
    public void SetNothing()
    {
        nametext.text = "Nothing";
        myitem = null; 
    }
    public void SelectItemAs()
    {
        if (handler != null)
        {
            handler.SelectItem(myitem);
        }
        if (inv != null)
        {
            inv.selectItem(myitem);
            Debug.Log("select item in inentory");
        }
    }
}
