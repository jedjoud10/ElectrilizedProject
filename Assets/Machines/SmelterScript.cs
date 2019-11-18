using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SmelterScript : MonoBehaviour
{
    public ItemAssetScript Item = null;
    public RawImage input;
    public ItemAssetScript outputItem;
    public RawImage output;
    public RecipeAssetScript[] recipes;
    // Start is called before the first frame update
    void Start()
    {
        if (Item != null)
        {
            Item = null;
        }
    }

    public void setItem(ItemAssetScript newitem)//Setting the correct item input
    {
        bool sucsess = true;
        if (newitem.Item_Type == ItemAssetScript.Item_Type_Enum.Ore)
        {            
            if (Item == null)
            {
                Item = newitem;
            }
            else
            {
                sucsess = false;
            }
        }
        if (newitem.Item_Type != ItemAssetScript.Item_Type_Enum.Ore)
        {
            sucsess = false;
        }
        if (sucsess)
        {
            Debug.Log("found item");
            GetComponent<MachineUIScript>().UseItem(newitem);
            changeInputs();
        }        
    }
    private void RemoveItem(ItemAssetScript old_item)//Deletes the existance of this item
    {
        if (Item == old_item)
        {
            Item = null;
        }
        changeInputs();
    }
    public void changeInputs()//Changes the actual GUI inputs
    {
        if (Item != null)
        {
            input.texture = Item.icon_texture;
        }
        else
        {
            input.texture = null;
        }        
        CheckItems();
    }
    public void returnItem()
    {
        if (Item != null)
        {
            if (GetComponent<MachineUIScript>().GiveItem(Item))
            {
                RemoveItem(Item);
            }
            else
            {
                Debug.Log("not enough space");
            }
        }
    }
    public void CheckItems()//Check for possible recipes
    {
        Debug.Log("check recipes");
        outputItem = null;
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i].inputs[0] == Item)
            {
                output.texture = recipes[i].output.icon_texture;
                outputItem = recipes[i].output;
                Debug.Log("Found posible craft for item" + recipes[i].output);
            }
        }
        if (outputItem == null)
        {
            output.texture = null;
        }
    }
    public void collectOutput()//Collect the output item
    {
        if (outputItem != null)
        {
            if (GetComponent<MachineUIScript>().GiveItem(outputItem))
            {
                if (Item != null)
                {
                    RemoveItem(Item);
                }
            }
            else
            {
                Debug.Log("not enough space");
            }
        }
    }
}
