using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using DG.Tweening;

public class ProjectUser_Space_INT : ScenarioItem
{

    public MeshRenderer webCamRenderer;
    public GameObject endQuad;

    public VideoPlayer videoPlayer;
    public float videoStartDelay = 10.0f;
    public bool needUser = true;

    override public void Start()
    {
        base.Start();

        endQuad.SetActive(false);

        if (videoPlayer)
        {
            DOTween.To((f) =>
            {

            }, 0, 1, 1).SetDelay(videoStartDelay).OnComplete(() =>
            {
                videoPlayer.playbackSpeed = 1;
                videoPlayer.gameObject.GetComponent<AudioSource>().Play();
            });
        }

        StartCoroutine("EnableUserVisualisation");
    }

    // Update is called once per frame
    override public void Update()
    {
        if (!needUser || KinectManager.Instance.GetUsersCount() > 0)
        {
            base.Update();
        }
    }

    public IEnumerator EnableUserVisualisation()
    {
        yield return new WaitForSeconds(0.1f);

        webCamRenderer.enabled = true;
    }
}
