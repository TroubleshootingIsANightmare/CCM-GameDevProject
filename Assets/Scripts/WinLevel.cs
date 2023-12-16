using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class WinLevel : MonoBehaviour
{
    public float winTime = 0f;

    public RespawnPlayer res;
    public bool rPaused;
    public UIManager manager;
    public TMP_Text winText;
    public GameObject winObject;
    public bool paused = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {


            winText.text = "Win Time: " + String.Format("{0:0.00}", winTime) + " Press up arrow to go to menu and space to play again.";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            paused = true;
        }
    }

    void Update()
    {
        rPaused = res.returnRPaused();
        if (paused && !rPaused) Time.timeScale = 0f; else if (!rPaused) Time.timeScale = 1f;
        if (paused && Input.GetKey(KeyCode.Space) && !rPaused) resetScene();
        if (!paused) winText.text = "";
    }


    void FixedUpdate()
    {
        winTime += Time.deltaTime;


    }
    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public bool returnWin()
    {
        return paused;
    }

}
