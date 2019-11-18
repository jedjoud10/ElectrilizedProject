using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolderingStationScript : MonoBehaviour
{
    public ItemAssetScript[] items = new ItemAssetScript[3];
    public RawImage[] inputs;
    public ItemAssetScript outputItem;
    public RawImage output;
    public RecipeAssetScript[] recipes;
    // Start is called before the first frame update
    void Start()
    {
        if (items.Length != 3)
        {
            items = new ItemAssetScript[3];
        }
    }
    public void setItem(ItemAssetScript newitem)//Setting the correct item input
    {
        bool sucsess = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null && !sucsess)
            {
                items[i] = newitem;
                sucsess = true;
            }
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
        bool finished = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == old_item && !finished)
            {
                items[i] = null;
                finished = true;
            }
        }
        changeInputs();
    }
    public void changeInputs()//Changes the actual GUI inputs
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                inputs[i].texture = items[i].icon_texture;
            }
            else
            {
                inputs[i].texture = null;
            }
        }
        CheckItems();
    }
    public void returnItem1()
    {
        if (items[0] != null)
        {
            if (GetComponent<MachineUIScript>().GiveItem(items[0]))
            {
                RemoveItem(items[0]);
            }
            else
            {
                Debug.Log("not enough space");
            }            
        }
    }
    public void returnItem2()
    {
        if (items[1] != null)
        {
            if (GetComponent<MachineUIScript>().GiveItem(items[1]))
            {
                RemoveItem(items[1]);
            }
            else
            {
                Debug.Log("not enough space");
            }
        }
    }
    public void returnItem3()
    {
        if (items[2] != null)
        {
            if (GetComponent<MachineUIScript>().GiveItem(items[2]))
            {
                RemoveItem(items[2]);
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
            if (ArraysDifference(items, recipes[i].inputs))
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
                for (int i = 0; i < items.Length; i++)
                {
                    RemoveItem(items[i]);
                }
            }
            else
            {
                Debug.Log("not enough space");
            }

        }
    }
    private bool ArraysDifference(ItemAssetScript[] a, ItemAssetScript[] b)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }
}
