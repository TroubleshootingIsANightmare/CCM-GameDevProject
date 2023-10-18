using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class UIManager : MonoBehaviour
{
    public Rigidbody rb;
    public KeyCode res = KeyCode.RightShift;
    public TMP_Text speed;
    public bool spd;
    public GameObject player;
    public Transform respawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(spd)
        {
            ShowSpeed();
        }  
        if(Input.GetKeyUp(res))
        {
            Restart();
        }
    }

    void ShowSpeed()
    {
        speed.text = "Speed: " + rb.velocity.magnitude;
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);


    }


}
