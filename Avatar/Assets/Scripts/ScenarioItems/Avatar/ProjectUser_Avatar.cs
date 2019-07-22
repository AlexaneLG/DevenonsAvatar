using UnityEngine;
using System.Collections;

public class ProjectUser_Avatar : ScenarioItem
{

    public MeshRenderer webCamRenderer;
    public GameObject endQuad;
    // public MeshRenderer avatar3D;
    public AudioSource WindSound;

    public GameObject videoPlane;

    public GameObject avatar;

    public bool needUser = true;

    public JoystickController joystickController;

    public bool isAvatarVisible;

    override public void Start()
    {
        base.Start();

        endQuad.SetActive(false);

        //WindSound.enabled = true;

        isAvatarVisible = false;

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
        yield return new WaitForSeconds(3f);

        if (joystickController)
        {
            joystickController.enabled = true;
        }

        if (videoPlane)
        {
            videoPlane.SetActive(true);
            isAvatarVisible = true;
        }

        if (avatar)
        {
            avatar.SetActive(true);
            isAvatarVisible = true;
        }


        if (KinectManager.Instance && KinectManager.Instance.GetUsersCount() > 0)
        {
            CharacterControllerBasedOnAxis.instance.AdjustAvatarScale();
        }
        else
        {

        }
    }
}
