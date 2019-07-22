using UnityEngine;
using System.Collections;

public class RetardEnable : MonoBehaviour {

    public float delay = 10.0f;

	public Renderer self;
	private float timer;
	private bool done;

	// Use this for initialization
	void Start () {
		timer = 0;
		self.enabled = false;
		done = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(done) return;
		timer += Time.deltaTime;
        if (timer > delay)
		{
			self.enabled = true;
			done = true;
		}
	}
}
