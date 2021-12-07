using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Cloud : MonoBehaviour
{
    //speed of the cloud
    public float moveSpeed;


    void Update()
    {
        //moves the clouds forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
}
