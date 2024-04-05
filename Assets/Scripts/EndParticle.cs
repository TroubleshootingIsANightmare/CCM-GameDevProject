using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParticle : MonoBehaviour
{
    public GameObject particle;
    public float life;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delete", life);
    }


    void Delete()
    {
        Destroy(particle);
    }

}
