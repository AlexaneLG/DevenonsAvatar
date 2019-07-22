using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

using U3D.Threading;
using U3D.Threading.Tasks;



using DG.Tweening;
using DG.Tweening.Core;

public class Navigation_Space_INT2 : Navigation_Space_INT
{

    public Camera mainCam;

    public Transform planeRotater;

    public Transform zoomTarget;

    public Material videoMaterial;

    public SpaceController spaceController;


    override public void Start()
    {
        base.Start();
        spaceController.useFlexion = false;
        Dispatcher.Initialize();
        DoBascule();
    }

    void DoBascule()
    {
        float time = 4f;

        mainCam.transform.DOLocalRotate(new Vector3(20, 0, 0), time).SetEase(Ease.InOutQuad);
        mainCam.transform.DOLocalMoveZ(-2f, time).SetEase(Ease.InOutQuad);

        planeRotater.transform.DOLocalRotate(new Vector3(60, 0, 0), time).SetEase(Ease.InOutQuad);

        //planeRotater.transform.DOLocalMoveY(0.2f, time).SetEase(Ease.InOutQuad);

        DOTween.To((f) =>
             {
                 spaceController.rollingEffectStrength = f;
             }, spaceController.rollingEffectStrength, 1, 5);


    }

    public override void Stop()
    {
        ajdustBaseSpeed(0);

        float time = 4f;

        mainCam.transform.DOLocalRotate(Vector3.zero, time).SetEase(Ease.InOutQuad);
        mainCam.transform.DOLocalMoveZ(-2.8f, time);

        planeRotater.transform.DOLocalRotate(Vector3.zero, time).SetEase(Ease.InOutQuad);
        planeRotater.transform.DOLocalMoveY(-0.2f, time).SetEase(Ease.InOutQuad);
        planeRotater.transform.DOLocalMoveX(-0.27f, time).SetEase(Ease.InOutQuad);

        Vector3 delta = zoomTarget.transform.position - mainCam.transform.position;

        Vector3 dest = mainCam.transform.position + delta * 0.975f;


        DOTween.To((f) =>
        {
            spaceController.multiplier = f;
        }, 1, 0, time).SetEase(Ease.InOutQuad);

        DOTween.To((f) =>
        {
            spaceController.rollingEffectStrength = f;
        }, 1, 0.33f, time).SetEase(Ease.InOutQuad);


        float delay = 12f;

        var blur = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();

        float zoomTime = 1;

        DOTween.To((f) =>
        {
            blur.blurSize = f;
        }, 0, 10, zoomTime).SetDelay(time + delay).SetEase(Ease.InQuad);

        mainCam.transform.DOMove(dest, zoomTime).SetDelay(time + delay).OnComplete(() =>
        {
            spaceController.rollingEffectStrength = 0f;
            DOTween.To((f) => { }, 0, 1, 1)
            .OnComplete(() =>
            {
                SensorRecorderManager.instance.endRecording(() =>
                {
                    Dispatcher.instance.ToMainThread(() =>
                    {
                        SceneManager.LoadScene("Foret");
                    });
                });
            });
        });
    }


}