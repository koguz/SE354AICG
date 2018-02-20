using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBoid : MonoBehaviour {

	private KArrive arrive;

	private int distance = 10;
	private int angle = 150;

	public Vector3 direction; 

	// Use this for initialization
	void Start () {
		arrive = GetComponent<KArrive> ();
		direction = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		if (arrive == null) {
			arrive = GetComponent<KArrive> ();
			return; 
		}

		// first, find your flockmates... 
		Vector3 target = Vector3.zero;
		Vector3 flockDir = Vector3.zero;
		Vector3 centerOfGravity = Vector3.zero;
		int flockMateCount = 0;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("boid")) {
			Transform t = g.transform;
			if (t == transform)
				continue;  // this is the object itself, skip it. 
			if ((t.position - transform.position).magnitude > distance)
				continue; // we are done with this boid
			
			// https://docs.unity3d.com/540/Documentation/ScriptReference/Vector3.Angle.html
			// Returns the angle in degrees between from and to.
			// The angle returned is always the non reflex angle between the two vectors - 
			// ie the smaller of the two possible angles between them and never greater than 180 degrees.
			if (Vector3.Angle (transform.forward, (t.position - transform.position)) > angle)
				continue; // we are done with this boid 

			// as Reynolds says, the boids that are far away from a distance, and an angle
			// are discarded as the flockmates. 

			// While I walk through the flockmates, I want to compute a target for the
			// KArrive script. 

			// There are three steering behaviours we have to consider. 
			// 1. separation: steer to avoid crowding local flockmates
			Vector3 fleeDirection = transform.position - t.position;
			float fdistance = fleeDirection.magnitude;
			float strength = 2.0f / (fdistance * fdistance); // the closer the boid, the further the target... 
			target += fleeDirection.normalized * strength;

			// 2. alignment 
			flockDir += t.forward * 5; // 2018 - i made a few magic number updates...

			// 3. center of gravity 
			centerOfGravity += t.position;

			flockMateCount++;
		}
		flockDir = flockDir / (float) flockMateCount;
		centerOfGravity = centerOfGravity / (float) flockMateCount;

		// now, we have to join target, flockDir and centerOfGravity
		arrive.target = target + flockDir + centerOfGravity; 
	}
}
