using UnityEngine;
using System.Collections;

public class End_Avatar : FreeFly_Avatar
{

    public GameObject endQuad;
    public GameObject videoCameraPlane;
    public GameObject greenScreen;

    Vector3 initialAvatarAngle;

    public override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine("DisableUserVisualisation");

        endQuad.SetActive(true);
        Color c = endQuad.GetComponent<Renderer>().sharedMaterial.GetColor("_TintColor");
        c = Color.black;
        c.a = 0;
        endQuad.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", c);

    }

    public override void OnDisable()
    {
        base.OnDisable();
        //endQuad.SetActive(false);
        if (SensorRecorderManager.instance.isRecording)
        {
            SensorRecorderManager.instance.endRecording();
        }
    }


    // Use this for initialization
    override public void Start()
    {
        base.Start();

        initialAvatarAngle = controller.avatar.localEulerAngles;

    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        Color c = endQuad.GetComponent<Renderer>().sharedMaterial.GetColor("_TintColor");
        c.a = progressionQuad;
        endQuad.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", c);

        var e = controller.avatar.localEulerAngles;
        float p = progressionQuad;

        e.x = Mathf.Lerp(initialAvatarAngle.x, 0, 4 * p);
        controller.avatar.localEulerAngles = e;
        //videoCameraPlane.transform.localEulerAngles = e;

        //greenScreen.renderer.sharedMaterial.SetFloat("_Taper", controller.taperFactor * (1f - p));
    }

    public IEnumerator DisableUserVisualisation()
    {
        yield return new WaitForSeconds(10f);

        videoCameraPlane.SetActive(false);
    }
}