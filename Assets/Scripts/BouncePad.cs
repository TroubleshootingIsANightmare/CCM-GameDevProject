using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    Rigidbody rb;
    public Transform pad;
    public int force = 10;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(pad.forward * rb.velocity.magnitude * force, ForceMode.Impulse);
        }
    }
}
