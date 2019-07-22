using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AvatarDisplay : MonoBehaviour
{

    public Transform target;
    public AvatarRunController runController;
    // Use this for initialization
    void Start()
    {

        var audioSource = GetComponent<AudioSource>();

        float l = audioSource.clip.length;

        (DOTween.Sequence()
               .Append(
                   target.DOLocalRotate(new Vector3(0, 180, 0), l / 2).SetEase(Ease.InQuad)
               )
               .Append(target.DORotate(new Vector3(0, 360, 0), l / 2).SetEase(Ease.OutQuad))
               )
               .SetDelay(2)
               .OnComplete(() =>
                {
                    runController.enabled = true;
                });

        DOTween.To((f) => { }, 0, 1, 8).OnComplete(() =>
        {
            audioSource.Play();

        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
