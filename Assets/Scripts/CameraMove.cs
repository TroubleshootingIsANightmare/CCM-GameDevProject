using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform orientation;
    public Camera cam;


    // Update is called once per frame
    void Update()
    {
        cam.transform.position = orientation.position;   
    }
}
