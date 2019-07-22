using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionForet : MonoBehaviour {

    public Navigation_Space_INT navigation;

    void OnTriggerEnter (Collider col)
	{
        if(col.transform.tag == "Avatar")
        {
           navigation.Stop();
        }
	}
}
