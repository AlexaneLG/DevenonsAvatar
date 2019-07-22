//SmoothLookAt.cs
//Written by Jake Bayer
//Written and uploaded November 18, 2012
//This is a modified C# version of the SmoothLookAt JS script.  Use it the same way as the Javascript version.

using UnityEngine;
using System.Collections;

///<summary>
///Looks at a target
///</summary>
[AddComponentMenu("Camera-Control/Smooth Look At CS")]
public class SmoothLookAt : MonoBehaviour
{
    public Transform target;		//an Object to lock on to
    public float damping = 6.0f;	//to control the rotation 
 
     private Transform _myTransform;

    void Awake()
    {
        _myTransform = transform;
    }

    void LateUpdate()
    {
        if (target)
        {
          
             //Look at and dampen the rotation
                Quaternion rotation = Quaternion.LookRotation(target.position - _myTransform.position);
                _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);
            
        }
    }
}