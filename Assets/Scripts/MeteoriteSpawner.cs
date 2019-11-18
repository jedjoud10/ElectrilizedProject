using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    public GameObject[] meteorites;
    public Vector2 randomtimerange;
    public float repeatrate;
    public BoxCollider basepos;
    public BoxCollider endpos;
    public GameObject handler;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RepeatRandomSpawning", 2, repeatrate);
    }

    private void SpawnMeteor()
    {
        GameObject meteorite = Instantiate(meteorites[Random.Range(0, meteorites.Length)], Vector3.zero, Quaternion.identity);
        meteorite.GetComponent<AsteroidScript>().base_pos = basepos;
        meteorite.GetComponent<AsteroidScript>().end_pos = endpos;
        meteorite.GetComponent<AsteroidScript>().asteroidTransformHandler = handler;
        meteorite.GetComponent<AsteroidScript>().StartAsteroid();
    }
    private void RepeatRandomSpawning()
    {        
        Invoke("SpawnMeteor", Random.Range(randomtimerange.x, randomtimerange.y));
    }
}
