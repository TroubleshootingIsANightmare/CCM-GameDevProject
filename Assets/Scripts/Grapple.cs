using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    private LineRenderer rope;
    private Camera cam;
    private float maxDistance = 100f;
    private Vector3 gPoint;
    private Vector3 targetPoint;
    private GameObject player;
    private SpringJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        rope = GetComponent<LineRenderer>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rope.enabled = false;
        gPoint = transform.position;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
       
        if(rope.enabled)
        {
            DrawRope();
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            StartGrapple();
        } 
        if(Input.GetKeyUp(KeyCode.Mouse2))
        {
            StopGrapple();
        }
    }

    void DrawRope()
    {
        gPoint = Vector3.Lerp(gPoint, targetPoint, Time.deltaTime * 8f);
        rope.SetPosition(0, transform.position);
        rope.SetPosition(1, gPoint);
    }

    void StopGrapple()
    {
        rope.enabled = false;
        Destroy(joint);
    }

    void StartGrapple()
    {
        RaycastHit hit;
        gPoint = transform.position;
        if (Physics.Raycast(cam.gameObject.transform.position, cam.gameObject.transform.forward, out hit, maxDistance))
        {
            targetPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = targetPoint;
            float distance = Vector3.Distance(player.transform.position, targetPoint);
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 5.0f;
        }
        rope.SetPosition(0, transform.position);
        joint.connectedAnchor = targetPoint;
        rope.SetPosition(1, targetPoint);
        rope.enabled = true;
    }
}
