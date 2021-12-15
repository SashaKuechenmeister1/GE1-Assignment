using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Behaviour : MonoBehaviour
{
    // controls the size of the clouds
    public float cloudSize;
    // maximun size clouds is going to grow to before shrinking
    public float cloudMaxSize;
    // minimum size clouds will shrink to before growing again
    public float cloudMinSize;

    // rate at which the clouds grow
    public float growRate; 
    // rate at which the clouds shrink
    public float shrinkRate;
    // cloud shrinks while this bool is true
    public bool shrink;

    
    // distance above the floor where clouds will hover
    public float hoverDistance;

    // point on the mesh where the Ray cast hits
    Vector3 hitPoint;
    // desired y position clouds lerp to
    float desiredYPos;
    
    
    // speed at which clouds lerp to the desired y position
    public float riseSpeed;

    // 2d texture that is used to pinpoint the pixel the cloud is over
    public Texture2D texture;
    // current colour of the pixel the raycast is hitting
    public Color col;
    // ray used to check height and colour
    Ray ray;
    // layer mask the ray is able to hit
    public LayerMask ground;
    // clouds model that shrinks
    public GameObject model;


    RaycastHit hit;

    void Start()
    {
        cloudSize = 1;
        cloudMaxSize = 2;
        cloudMinSize = 0.8f;
    }

    void Update()
    {
        model.transform.localScale = new Vector3(cloudSize, cloudSize, cloudSize);
        AvoidGround();
        SetInactive();
        SampleColourUnder();
        
        //if the cloud hits max size it will start shrinking
        if(cloudSize >= cloudMaxSize){
            shrink = true;
        }


        // happens while the cloud is shrinking
        if(shrink){

            // clouds size shrinks
            cloudSize -= shrinkRate *Time.deltaTime;

            // flips shrink when cloud hit minimum size
            if(cloudSize <= cloudMinSize){
                shrink = false; 
            }
        }else{
            // slowly grows cloud while the cloud isn't shrinking
            cloudSize += (growRate/15f) * Time.deltaTime;
        }


    }

    // deletes the cloud when it sinks too low
    void SetInactive(){
        if(transform.position.y < 0){
            this.gameObject.SetActive(false);
        }
    }


    // function that lets clouds avoid the floor
    void AvoidGround(){
        if(hitPoint != Vector3.zero){
            Vector3 desiredPos = new Vector3(hitPoint.x, hitPoint.y + hoverDistance, hitPoint.z);

            transform.position = Vector3.Lerp(transform.position, desiredPos, riseSpeed * Time.deltaTime);
        }
    }


    // function that shoots the ray, samples the color and location it hits
    void SampleColourUnder(){
        Debug.DrawRay(transform.position, Vector3.down *300f, Color.red);

        ray = new Ray(transform.position, Vector3.down);

        // shoots ray straight down
        if(Physics.Raycast(ray, out hit, 300f, ground)){
            // gets the hits exact pixel colour
            if(hit.collider.tag.Equals("Ground")){
                texture = hit.transform.GetComponent<Renderer>().material.mainTexture as Texture2D;
                Vector2 pixelHit = hit.textureCoord;
                pixelHit.x *= texture.width;
                pixelHit.y *= texture.height;

                col = texture.GetPixel((int)pixelHit.x, (int)pixelHit.y);
                //sets the vector of the hit position
                hitPoint = hit.point;
            }
        }
        else{
            hitPoint = new Vector3();
        }

    }

}
