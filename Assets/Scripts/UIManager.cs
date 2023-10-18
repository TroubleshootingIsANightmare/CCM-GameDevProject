using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Rigidbody rb;
    public TMP_Text speed;
    public bool spd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spd)
        {
            ShowSpeed();
        }  
    }

    void ShowSpeed()
    {
        speed.text = "Speed: " + rb.velocity.magnitude;
    }

}
