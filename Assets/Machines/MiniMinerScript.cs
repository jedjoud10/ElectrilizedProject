using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMinerScript : MonoBehaviour
{
    public GameObject player;
    public MinableAssetScript minablepreset;
    public float waterlevel;
    public GameObject failexplosion;
    public float explosiontime;
    private void Start()
    {
        Invoke("startmachine", 2);
    }
    public void startmachine()
    {
        if (player == null){return;}
        if (transform.position.y > waterlevel || Random.value > 0.5)
        {
            Instantiate(failexplosion, transform.position, transform.rotation);
            Invoke("destroymachine", explosiontime);
            return;
        }
        player.GetComponent<InventoryScript>().AddItem(minablepreset.items[Random.Range(0, minablepreset.items.Length)]);        
        Invoke("stopmachineanddestroy", 1);
    }
    public void destroymachine()
    {
        Destroy(gameObject);
    }
    public void stopmachineanddestroy()
    {
        Instantiate(failexplosion, transform.position, transform.rotation);
        Invoke("destroymachine", explosiontime);
    }
}
