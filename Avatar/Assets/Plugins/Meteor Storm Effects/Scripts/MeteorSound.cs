using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorSound : MonoBehaviour 
{

    public GameObject particleAudioSource;
    
    public ParticleSystem emitter;
    public ParticleSystem.Particle[] allParticles;
    public List<GameObject> particulesSounds;

    void Setup()
    {
        allParticles = new ParticleSystem.Particle[emitter.maxParticles]; 
        particulesSounds = new List<GameObject>();
        for (int i = 0; i < emitter.maxParticles; i++)
        {
            particulesSounds.Add(Instantiate(particleAudioSource, Vector3.zero, Quaternion.identity) as GameObject);
            particulesSounds[i].transform.parent = this.transform;
        }
    }
    
	// Update is called once per frame
	void LateUpdate () 
    {
        if (particulesSounds.Count == 0)
            Setup();

        int nbParticules = emitter.GetParticles(allParticles);

        for (int i = 0; i < particulesSounds.Count ; i++)
        {
            //Debug.Log("Particule " + i + " : " + allParticles[i].position);
            if (i < nbParticules)
            {
                particulesSounds[i].SetActive(true);

                // Translation sur position emmiter + orientation de la position du prefab selon l'orientation de l'emmiter.
                // La multiplication d'un quaternion par un vecteur oriente le vecteur suivant l'orientation du quaternion.
                particulesSounds[i].transform.position = emitter.transform.position + emitter.transform.rotation 
                    * allParticles[i].position;

                // Translation sur position emmiter + orientation de la position du prefab selon l'orientation de l'emmiter.
                // Modification de repère local en repère world.
                /*particulesSounds[i].transform.position = emitter.transform.localToWorldMatrix 
                    * allParticles[i].position;*/
            }
            else
                particulesSounds[i].SetActive(false);
        }
	}
}
