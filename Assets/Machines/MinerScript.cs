using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerScript : MonoBehaviour
{
    public MinableAssetScript[] minableObjects;
    public ItemAssetScript[] items = new ItemAssetScript[5];
    public MinerItem[] gui_itemholders;
    public int maxLevel = 300;
    public int currentlevel = 0;
    public int steps = 1;
    public float delaybetweensteps = 1;
    public Text metersText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void startmining()
    {
        StartCoroutine(startminingcoroutine());
        items = new ItemAssetScript[5];
        updateGUI();
    }
    public IEnumerator startminingcoroutine()
    {
        currentlevel = 0;
        while (currentlevel < maxLevel)
        {
            currentlevel += steps;
            mineLevel(currentlevel);
            yield return new WaitForSeconds(delaybetweensteps);
        }
    }
    public void mineLevel(int level)
    {
        Debug.Log("mining at level" + level);
        metersText.text = level + " Meters"; 
        for (int i = 0; i < minableObjects.Length; i++)
        {
            if (minableObjects[i].surfacedistance_min < level && minableObjects[i].surfacedistance_max > level)
            {
                Debug.Log("mining level is in range");
                for (int j = 0; j < minableObjects[i].items.Length; j++)
                {
                    if (Random.Range(0, minableObjects[i].probability) == 0)
                    {
                        additem(minableObjects[i].items[j]);
                        Debug.Log("adding item to local items");
                    }
                }
            }
        }
    }
    public void additem(ItemAssetScript item)//overwrites the item
    {
        bool sucsess = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null && !sucsess)
            {
                items[i] = item;
                sucsess = true;
                updateGUI();
                return;
            }
        }
        if (!sucsess)
        {
            items[Random.Range(0, items.Length)] = item;
            updateGUI();
        }       
    }
    public void removeitem(ItemAssetScript item)//stop the very existance of this poor item
    {
        bool sucsess = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item && ! sucsess)
            {
                items[i] = null;
                updateGUI();
                sucsess = true;
            }
        }
    }
    public void recoltItem(ItemAssetScript item)
    {
        if (GetComponent<MachineUIScript>().GiveItem(item))
        {
            removeitem(item);
            updateGUI();
        }
        else
        {
            Debug.Log("not enough space");
        }
    }
    public void updateGUI()
    {
        for (int i = 0; i < gui_itemholders.Length; i++)
        {
            if (items[i] != null)
            {
                gui_itemholders[i].setItem(items[i]);
            }
            else
            {
                gui_itemholders[i].setnothing();
            }
        }
    }
}
