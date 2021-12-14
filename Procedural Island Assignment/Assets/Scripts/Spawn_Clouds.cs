using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Clouds : MonoBehaviour
{
    // used to access the obj pools
    Object_Pool pools;

    // length of time the spawner waits before spawning another cloud
    public float spawnRate;

    void Start()
    {
        pools = Object_Pool.Instance;
        StartCoroutine(SpawnCloud());
    }

    // this coroutine spawns the clouds
    IEnumerator SpawnCloud(){
        while(true){
            // spawnpoint in local space
            Vector3 spawnPoint = new Vector3(Random.Range(-1000, 1000), 0, 0);

            // spawnpoint in world space
            Vector3 localSpawn = transform.TransformPoint(spawnPoint);
            
            // spawns clouds out of the Cloud pool
            pools.SpawnFromPool("Cloud", localSpawn, transform.rotation);
            yield return new WaitForSeconds(spawnRate);
        }

    }
}
