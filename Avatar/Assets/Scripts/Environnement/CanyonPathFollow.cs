using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CanyonPathFollow : MonoBehaviour {

    public iTweenPath path;

    [Range(0f,1f)]
    public float pathProgression = 0.0f;


    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 sp = iTween.PointOnPath(path.nodes.ToArray(), pathProgression);
        transform.localPosition = sp;

	}
}
