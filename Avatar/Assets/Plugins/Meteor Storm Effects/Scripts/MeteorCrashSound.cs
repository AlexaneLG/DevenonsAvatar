using UnityEngine;
using System.Collections;

public class MeteorCrashSound : MonoBehaviour {

public AudioClip CrashSound;

private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

	void OnParticleCollision(GameObject other) {
		int safeLength = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
		if (collisionEvents.Length < safeLength)
			collisionEvents = new ParticleCollisionEvent[safeLength];
		
		int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
		int i = 0;
		while (i < numCollisionEvents) {
			Vector3 CrashLocation = collisionEvents[i].intersection;
			AudioSource.PlayClipAtPoint(CrashSound, CrashLocation);
			
			i++;
		}
	}
}

