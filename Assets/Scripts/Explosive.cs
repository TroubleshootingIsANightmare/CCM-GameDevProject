using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float lifeTime = 5f;
    public float power = 50f;
    public float radius = 4f;
    public int hitLayer = 3;
    public bool canExplode;
    public GameObject rocket;
    public GameObject explosiveFX;
    public float knockbackForce = 50f;
    // Start is called before the first frame update
    void Start()
    {
        rocket = this.gameObject;
        Invoke("Delete", lifeTime);
    }

    void Delete()
    {
        Destroy(rocket);
    }



    void OnTriggerEnter(Collider other)
    {
        if (canExplode)
        {
            
            if (other.gameObject.layer == hitLayer)
            {
                Explode();

            }
        }
    }

    void Explode()
    {
        
        Vector3 explosionPos = transform.position;
        Collider[] hits = Physics.OverlapSphere(explosionPos, radius);
        Instantiate(explosiveFX, rocket.transform.position, rocket.transform.rotation);
        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
 

            if (rb != null)
            {
                Vector2 locate = new Vector2(hit.gameObject.transform.position.x, hit.gameObject.transform.position.z);
                Vector2 direction = new Vector2(locate.x - explosionPos.x, locate.y - explosionPos.z);
                direction.Normalize();
                Destroy(rocket);
                rb.AddForce(new Vector3(direction.x * knockbackForce, 0, direction.y * knockbackForce));
                rb.AddExplosionForce(power, explosionPos, radius, 4);
            }

            
        }
    }
}
