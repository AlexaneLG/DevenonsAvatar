using UnityEngine;
using System.Collections;

public class GlobalTriggerDetection : MonoBehaviour {

    public CollisionManager collisionManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Meteor" && collisionManager != null)
        {
            collisionManager.MeteorCrash();
        }
            
    }
}
