using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WatchMenu : MonoBehaviour
{
    public void WatchOffOff()
    {
        SceneManager.LoadScene("offvoff");
    }

    public void WatchDefOff()
    {
        SceneManager.LoadScene("offvsdef");
    }

    public void WatchDefDef()
    {
        SceneManager.LoadScene("defvsdef");
    }
}
