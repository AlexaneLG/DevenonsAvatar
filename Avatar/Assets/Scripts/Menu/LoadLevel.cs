using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class LoadLevel : MonoBehaviour
{


    public void LoadPlayer()
    {
        SceneManager.LoadScene("Player");
    }

    public void LoadSpace()
    {
        SceneManager.LoadScene("SpaceINT");
    }

    public void LoadBoard()
    {
        SceneManager.LoadScene("Board");
    }

    public void LoadSpaceForet()
    {
        SceneManager.LoadScene("SpaceINT-2");
    }

    public void LoadAvatar()
    {
        SceneManager.LoadScene("Avatar");
    }

    public void LoadAvatar_JV()
    {
        SceneManager.LoadScene("Avatar-JV");
    }

    public void LoadAvatar_JV_FP()
    {
        SceneManager.LoadScene("Avatar-JV-FP");
    }

    public void LoadAvatar_FP()
    {
        Application.LoadLevel("Avatar-FP");
    }
}
