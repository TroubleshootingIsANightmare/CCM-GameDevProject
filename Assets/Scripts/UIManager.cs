using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public Rigidbody rb;
    public KeyCode res = KeyCode.RightShift;
    public KeyCode menu = KeyCode.M;
    public TMP_Text speed;
    public TMP_Text controlText;

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowSpeed();
        
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
        speed.text = "Speed: " + String.Format("{0:0.00}", rb.velocity.magnitude);
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


}
