using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float lifeTime;
    public float power;
    public float radius;
    public int hitLayer;
    public int explodeLayer;
    public GameObject rocket;
    public GameObject explosiveFX;
    public float knockbackForce;
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
        Debug.Log("Explode");
        if(other.gameObject.layer == hitLayer)
        {
            Explode();
        } else if(other.gameObject.tag == "Player")
        {
            return;
        }
    }

    void Explode()
    {
        
        Vector3 explosionPos = transform.position;
        Collider[] hits = Physics.OverlapSphere(explosionPos, radius);
        
        foreach (Collider hit in hits)
        {
            Debug.Log("Hit");
            Rigidbody rb = hit.GetComponent<Rigidbody>();
 

            if (rb != null)
            {
<<<<<<< Updated upstream
                Vector3 locate = new Vector3(hit.gameObject.transform.position.x, 0f, hit.gameObject.transform.position.z);
                Vector3 direction = locate - explosionPos;
                Destroy(rocket);
                rb.AddForce(direction.x * knockbackForce, 0, direction.z * knockbackForce);
                rb.AddExplosionForce(power, explosionPos, radius, 0);
=======
                Vector2 locate = new Vector2(hit.gameObject.transform.position.x, hit.gameObject.transform.position.z);
                Vector2 direction = new Vector2(locate.x - explosionPos.x, locate.y - explosionPos.z);
                direction.Normalize();
                Destroy(rocket);
                rb.AddForce(new Vector3(direction.x * knockbackForce, 0,direction.y * knockbackForce));
                rb.AddExplosionForce(power, explosionPos, radius, 1);
>>>>>>> Stashed changes
            }

            Instantiate(explosiveFX, rocket.transform.position, rocket.transform.rotation);
        }
    }
}
