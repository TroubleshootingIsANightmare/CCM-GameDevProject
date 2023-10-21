using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public Rigidbody rb;
    public KeyCode res = KeyCode.RightShift;
    public KeyCode menu = KeyCode.UpArrow;
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
        if (Input.GetKeyUp(menu))
        {
            SceneManager.LoadScene(2);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
