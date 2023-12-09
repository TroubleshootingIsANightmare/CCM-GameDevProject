using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

public class WinLevel : MonoBehaviour
{
    public float winTime;
    public Timer timer;
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
            winTime = timer.returnTime();
            Instantiate(winText.gameObject, GameObject.Find("PlayerUI").transform);
            winObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You beat the level in " + Time.timeSinceLevelLoad + " seconds! Press the up arrow to return to menu and any other key to restart the level.";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            paused = true;
        }
    }

    void Update()
    {
        rPaused = res.returnRPaused();
        if (paused && !rPaused) Time.timeScale = 0f; else if(!rPaused)Time.timeScale = 1f;
        if (paused && Input.anyKey && !rPaused) Invoke("resetScene", 3f);
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
