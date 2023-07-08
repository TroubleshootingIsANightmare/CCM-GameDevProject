using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float lifeTime;
    public float power;
    public float radius;
    public int hitLayer;
    public int explosionLayer;
    private Collider[] hits;
    public GameObject rocket;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delete", lifeTime);
    }

    void Delete()
    {
        Destroy(rocket);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.layer == hitLayer)
        {
            Explode();

        }
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        hits = Physics.OverlapSphere(explosionPos, radius, explosionLayer);
        foreach (Collider hit in hits)
        {
            Debug.Log("Hit");
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log("Ow");
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f);
            }
        }
    }
}
