using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class RespawnPlayer : MonoBehaviour
{
    public TMP_Text death;
    public bool paused;
    public WinLevel winScript;
    public bool pWin = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pWin = winScript.returnWin();
        if(!pWin && paused) Time.timeScale = 0f; else if (!pWin) Time.timeScale = 1f;
        if(paused && Input.anyKey && !pWin) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Instantiate(death.gameObject, GameObject.Find("PlayerUI").transform);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
      
            paused = true;
        }
    }

    public bool returnRPaused()
    {
        return paused;
    }

  


}
