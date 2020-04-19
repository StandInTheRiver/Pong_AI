using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public void PlayDefGame()
    {
        SceneManager.LoadScene("playervsdef");
    }

    public void PlayOffGame()
    {
        SceneManager.LoadScene("playervsoff");
    }
}
