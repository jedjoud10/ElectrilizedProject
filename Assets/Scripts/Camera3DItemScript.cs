using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3DItemScript : MonoBehaviour
{
    public Vector3 speed;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(speed);
    }
    public void selectItem(ItemAssetScript item)
    {
        if (item == null)
        {
            return;
        }
        GetComponent<MeshFilter>().sharedMesh = item.Model;
        GetComponent<MeshRenderer>().materials = item.Mesh_Materials;
        cam.orthographicSize = item.CameraFOV + 5;
    }
    public void deselect()
    {
        GetComponent<MeshFilter>().sharedMesh = null;        
    }
}
