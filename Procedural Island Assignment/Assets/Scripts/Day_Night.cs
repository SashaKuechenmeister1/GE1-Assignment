using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_Night : MonoBehaviour
{
    // mesh which the sun and moon directional light revolve around
    GameObject mesh;

    void Start()
    {
        mesh = GameObject.Find("Mesh");
    }

    void Update()
    {
        // rotates the sun and moon around the mesh
        transform.RotateAround(mesh.transform.position, Vector3.forward, 10f * Time.deltaTime);
        
        // keeps the sun and moon light looking down at the mesh
        transform.LookAt(mesh.transform.position);
    }
}
