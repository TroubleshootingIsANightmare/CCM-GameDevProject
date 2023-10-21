using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform respawn;
    public Rigidbody rb;
    public Timer time;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = respawn.position;
            rb = other.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, 0);
            time.i = 0f;
            other.gameObject.transform.rotation = Quaternion.identity;
        }
    }
}
