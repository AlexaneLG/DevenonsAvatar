using UnityEngine;
using System.Collections;

public class ProjectUser_Wii : ScenarioItem {

    public MeshRenderer webCamRenderer;
    public GameObject endQuad;

    public bool needUser = true; 

	override public void Start () 
    {
        base.Start();

        endQuad.SetActive(false);

        StartCoroutine("EnableUserVisualisation");
	}
	
	// Update is called once per frame
	override public void Update () {
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
