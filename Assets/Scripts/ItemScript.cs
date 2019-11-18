using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemAssetScript MyItem;
    void Start()
    {
        setItemValues();
    }
    public void setItemValues()
    {
        if (MyItem != null)
        {
            GetComponent<MeshFilter>().sharedMesh = MyItem.Model;
            GetComponent<MeshRenderer>().materials = MyItem.Mesh_Materials;
            GetComponent<MeshCollider>().sharedMesh = MyItem.Model;
        }
    }
}
