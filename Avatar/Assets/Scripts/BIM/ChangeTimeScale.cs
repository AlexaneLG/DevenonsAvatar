using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScale : MonoBehaviour {

    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void playGame()
    {
        Time.timeScale = 1f;
    }

    public void slowGame()
    {
        Time.timeScale = 0.5f;
    }

    public void speedGame()
    {
        Time.timeScale = 1.5f;
    }

    public void speedGameToNextScenario ()
    {
        Time.timeScale = 1.5f;
        // stop condition
    }
}
