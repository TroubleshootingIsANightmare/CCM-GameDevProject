using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    public float offset;
    public float intensity;
    public Transform originalOffset;
    public Transform cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        if(inputVector.magnitude > 0f)
        {
            offset += Time.deltaTime * intensity;
            
        } else
        {
            offset = 0f;
        }

        float offsetY = -Mathf.Abs(intensity * Mathf.Sin(offset));
        Vector3 offsetX = originalOffset.right * intensity * Mathf.Cos(offset) * intensity;

        originalOffset.position = new Vector3(originalOffset.position.x, originalOffset.position.y * offsetY, originalOffset.position.z);

        originalOffset.position += offsetX;
    }
}
