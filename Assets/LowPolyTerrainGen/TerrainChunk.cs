using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    public Vector3 position;
    public int myBiome;
    public void SetupChunk(Vector2Int id, int resolution, int mybiome)
    {
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshCollider>();
        myBiome = mybiome;
        if (GetComponent<MeshRenderer>().sharedMaterial == null)
        {
            GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        }
        gameObject.name = id.x + "-" + id.y;
        position = new Vector3(id.x * resolution, 0, id.y * resolution);
    }
    public void SetChunk(Mesh mesh, Texture texture, GameObject detailspawner, GameObject water)
    {
        if (GetComponent<MeshRenderer>().sharedMaterial != null)
        {
            GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", texture);
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Glossiness", 0);            
            int res = GetComponent<MeshRenderer>().sharedMaterial.GetTexture("_MainTex").width;
            GetComponent<MeshRenderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(1.0f / res, 1.0f / res));
        }
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        if (detailspawner != null)
        {
            GameObject details = Instantiate(detailspawner);
            details.transform.position = gameObject.GetComponent<MeshFilter>().mesh.bounds.center;
            details.transform.parent = gameObject.transform;
            details.GetComponent<DetailGenerator>().water = water;
        }
    }    
}
