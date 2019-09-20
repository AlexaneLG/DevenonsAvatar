using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ChangeTimeScale : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void pauseGame()
    {
        videoPlayer.Pause();
        Time.timeScale = 0f;
    }

    public void playGame()
    {
        videoPlayer.Play();
        Time.timeScale = 1f;
    }

    public void slowGame()
    {
        videoPlayer.playbackSpeed = 0.5f;
        Time.timeScale = 0.5f;
    }

    public void speedGame()
    {
        videoPlayer.playbackSpeed = 1.5f;
        Time.timeScale = 1.5f;
    }

    public void speedGameToNextScenario()
    {
        videoPlayer.playbackSpeed = 1.5f;
        Time.timeScale = 1.5f;
        // stop condition
    }
}
