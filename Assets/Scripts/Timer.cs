using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public TMP_Text _time;
    public float i;
    // Start is called before the first frame update
    void Start()
    {
        _time = GetComponent<TMP_Text>();
        i = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        i += Time.deltaTime;
        _time.text = "Time: " + String.Format("{0:0.00}", i); 
    }


    public float returnTime()
    {
        return i;
    }
}
