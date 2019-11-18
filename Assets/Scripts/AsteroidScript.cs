using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public BoxCollider base_pos;
    public Rigidbody rigidbody_variable;
    private Vector3 basePosVector;
    private Vector3 endPosVector;
    public BoxCollider end_pos;
    public Vector2 speed_range;
    public GameObject asteroidTransformHandler;
    public ItemAssetScript[] items;
    public GameObject item_holder;
    public float explosion_force;
    public GameObject explosion_particles;
    public float particlesTime;
    public void StartAsteroid()
    {
        basePosVector = new Vector3(Random.Range(base_pos.bounds.min.x, base_pos.bounds.max.x), Random.Range(base_pos.bounds.min.y, base_pos.bounds.max.y), Random.Range(base_pos.bounds.min.z, base_pos.bounds.max.z));
        endPosVector = new Vector3(Random.Range(end_pos.bounds.min.x, end_pos.bounds.max.x), Random.Range(end_pos.bounds.min.y, end_pos.bounds.max.y), Random.Range(end_pos.bounds.min.z, end_pos.bounds.max.z));
        rigidbody_variable.position = basePosVector;
        transform.position = basePosVector;
        asteroidTransformHandler.transform.position = endPosVector;
        rigidbody_variable.transform.LookAt(asteroidTransformHandler.transform);
        rigidbody_variable.AddForce(rigidbody_variable.transform.forward * Random.Range(speed_range.x, speed_range.y), ForceMode.Force);
    }
    private void Start()
    {
        if (Debug.isDebugBuild)
        {
            //StartAsteroid();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < items.Length; i++)
        {
            GameObject newItem = Instantiate(item_holder, rigidbody_variable.position, Quaternion.identity);
            newItem.GetComponent<ItemScript>().MyItem = items[i];
            newItem.GetComponent<ItemScript>().setItemValues();
            newItem.GetComponent<Rigidbody>().AddExplosionForce(explosion_force, rigidbody_variable.position, 99999);
        }
        GameObject particles = Instantiate(explosion_particles, rigidbody_variable.position, Quaternion.identity);
        Destroy(particles, particlesTime);
        Destroy(gameObject);
    }
}
