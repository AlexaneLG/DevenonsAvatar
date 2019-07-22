using UnityEngine;
using System.Collections;

public class AvatarFlightController : MonoBehaviour
{

    protected CharacterControllerBasedOnAxis controller;

	// Use this for initialization
	public virtual void Start () {
        controller = CharacterControllerBasedOnAxis.instance;
	}
	
}
