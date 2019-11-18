using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class MinableAssetScript : ScriptableObject
{
    public int probability;
    public int surfacedistance_min;
    public int surfacedistance_max;
    public ItemAssetScript[] items;
}
