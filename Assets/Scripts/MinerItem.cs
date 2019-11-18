using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerItem : MonoBehaviour
{
    public RawImage image;
    public Text text;
    public ItemAssetScript item;
    public MinerScript miner;
    public void setItem(ItemAssetScript newitem)
    {
        item = newitem;
        text.text = item.name.Replace("_", " ");
        image.texture = item.icon_texture;
    }
    public void setnothing()//remove text and texture
    {
        item = null;
        text.text = "";
        image.texture = null;
    }
    public void recoltItem()//recolt the item from the miner machine
    {
        miner.recoltItem(item);
    }    
}
