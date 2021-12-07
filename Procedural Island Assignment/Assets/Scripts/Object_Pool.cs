using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pool : MonoBehaviour
{

    //the holder for the object pools
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    //List of Pool class which holds the type of the object, the prefab associated with that object type and the amount GameObject in the pool
    public List<Pool> pools;
    //singleton to access pools easily
    public static Object_Pool Instance;

    void Awake(){
        //Assigns instance
        Instance = this;
    }
    
    void Start()
    {
        //initiallises the object pools as a string and queue
        //the string is the object type
        //the queue is the data structure to hold the objects. Queues are fast to grab the first item out of them which makes them ideal for spawning things out of them
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //sets up the pools and instanciates the disables objects in the scene
        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++){
                GameObject obj = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            //adds the pool to the dictionary
            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    //function that can be accessed through the singleton to grab objects out of a specific pool and place it somewhere in the scene
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation){
        //The warning that displays if the pool trying to get accessed doesn't exist
        if(!poolDictionary.ContainsKey(tag)){
            Debug.LogWarning("Doesn't exist");
            return null;
        }

        //Grabs the first item in the queue
        GameObject obj = poolDictionary[tag].Dequeue();
        //sets the object active and places it
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        //then places it back in the queue
        poolDictionary[tag].Enqueue(obj);

        return obj;
    }

    
    //This is the class that holds all of the infomation needed to create an object pool

    //Lets the class be seen in editor
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }
}
