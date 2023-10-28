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
    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(paused) Time.timeScale = 0f; else Time.timeScale = 1f;
        if(paused && Input.anyKey) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        

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


}
