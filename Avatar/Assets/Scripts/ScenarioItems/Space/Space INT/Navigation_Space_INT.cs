using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class Navigation_Space_INT : ScenarioItem
{
    public GameObject User;

    public float pathSpeed = 0.25f;
    public float baseSpeed = 0.25f;

    public float decay = 0.05f;

    private float pathDistance = 0f;
    private Vector3[] thePath;
    private float pathLength;


    override public void Start()
    {
        base.Start();

        thePath = iTweenPath.GetPath("StationPath");
        pathLength = iTween.PathLength(thePath);

        // Ancienne méthode
        //iTween.MoveTo(User, iTween.Hash("path", iTweenPath.GetPath("StationPath"), "axis", "z", "time", 1000f, "orienttopath", true)); 
    }


    override public void Update()
    {
        base.Update();

        pathDistance += pathSpeed * Time.deltaTime;
        float perc = pathDistance / pathLength;
        iTween.PutOnPath(User, thePath, perc);

        pathSpeed -= Time.deltaTime * decay;

        if (pathSpeed < baseSpeed)
        {
            pathSpeed = baseSpeed;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("SpaceEXT");
        }
    }

    public virtual void Stop()
    {
        controller.ActivateNextScenarioItem();
    }

    public void ajdustBaseSpeed(float newBaseSpeed)
    {
        baseSpeed = newBaseSpeed;
    }
}