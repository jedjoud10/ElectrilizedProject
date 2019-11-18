using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class LODTerrain : MonoBehaviour
{
    public PlayerMechanics player;
    public float gridsize;
    private Vector3 position;
    private Vector3 oldposition;
    private bool hasmooved;
    public float MaxDistance;
    public Vector3 offset;
    public int CurrentBiome = 0;
    public GameObject[] chunks;
    private RaycastHit hit;
    public void Update()
    {
        position = new Vector3(SnapTo(gameObject.transform.position.x, gridsize), SnapTo(gameObject.transform.position.y, gridsize), SnapTo(gameObject.transform.position.z, gridsize));
        hasmooved = oldposition != position;
        oldposition = position;
        if (hasmooved)
        {
            ReloadChunksDistance();
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.GetComponent<TerrainChunk>() != null)
            {
                CurrentBiome = hit.collider.gameObject.GetComponent<TerrainChunk>().myBiome;
                player.ChangeBiome(CurrentBiome);
            }
        }
    }
    public static float SnapTo(float a, float snap)
    {
        snap *= 0.1f;
        return Mathf.Round(a / snap) * snap;
    }
    public void ReloadChunksDistance()
    {
        float dist = 0;
        Vector3 mypos = transform.position;
        foreach (GameObject chunk in chunks)
        {
            dist = Vector3.Distance(mypos + offset, chunk.GetComponent<MeshRenderer>().bounds.center);
            chunk.GetComponent<MeshRenderer>().enabled = (dist < MaxDistance);
            chunk.transform.GetChild(0).gameObject.SetActive((dist < MaxDistance));
        }     
    }
}
