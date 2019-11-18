using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class RecipeAssetScript : ScriptableObject
{
    public ItemAssetScript[] inputs;
    public ItemAssetScript output;
}
