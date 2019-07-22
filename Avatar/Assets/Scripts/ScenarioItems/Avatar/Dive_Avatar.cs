using UnityEngine;
using System.Collections;

public class Dive_Avatar : ScenarioItem
{
    public bool basculeAuto = true;

    public Transform skyCam;
    public Transform videoCameraPlane;
    public GameObject greenScreen;


    private bool enableDive = true;

    private float lerpTime = 3f;
    private float currentLerpTime = 0;

    private bool basculeStarted = false;

    // Use this for initialization
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        if (basculeAuto || basculeStarted)
        {
            base.Update();

            if (self.eulerAngles.x < controller.maxBascule)
            {

                Vector3 e = self.localEulerAngles;
                float p = progressionQuad;

                e.x = Mathf.Lerp(0, controller.maxBascule, p);
                self.localEulerAngles = e;
                skyCam.localEulerAngles = e;

                e.x *= 2;
                controller.avatar.localEulerAngles = e;
                //videoCameraPlane.localEulerAngles = e;

                //greenScreen.renderer.sharedMaterial.SetFloat("_Taper", controller.taperFactor * p);

                self.Translate(p * Time.deltaTime * controller.speed * Vector3.forward, Space.World);
            }
        }
        /*else
        {
            // Detecter l'inclinasion de l'utilisateur pour lancer la bascule
            if (controller.angle.currentValue >= controller.angle.max/4)
            {
                basculeStarted = true;
                Debug.Log("Bascule Detected");
            }
        }*/

    }
}
