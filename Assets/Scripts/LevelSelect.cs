using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    // Update is called once per frame
    public void OpenScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
