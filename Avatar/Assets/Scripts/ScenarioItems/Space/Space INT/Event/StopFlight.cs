using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class StopFlight : MonoBehaviour
{

    public Navigation_Space_INT2 navigation;



    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Avatar")
        {
            navigation.Stop();
        }
    }
}
