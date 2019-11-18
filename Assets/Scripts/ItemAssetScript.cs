using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class ItemAssetScript : ScriptableObject
{
    public Texture icon_texture;
    public Mesh Model;
    public Material[] Mesh_Materials;
    public string description;
    public GameObject Machine;
    public float CameraFOV;
    public enum Item_Type_Enum
    {
        Ore,
        Metal,
        Component,
        Machine
    }
    public Item_Type_Enum Item_Type;
}
