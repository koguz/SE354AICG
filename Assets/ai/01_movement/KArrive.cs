using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Short for Kinematic Arrive
public class KArrive : MonoBehaviour {

	public Vector3 target;
	private int maxSpeed = 10; 
	private float radius = 0.1f;

	// Use this for initialization
	void Start () {
		target = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if ( (transform.position - target).magnitude > radius ) {
			// find the direction
			Vector3 direction = (target - transform.position);
			float speed = direction.magnitude * 4;
			direction.Normalize ();

			// rotate
			float a = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0, a, 0);

			// compute velocity
			if (speed > maxSpeed) {
				speed = maxSpeed; 
			}
			Vector3 velocity = direction * speed;

			// translate with time.deltatime
			transform.Translate(velocity * Time.deltaTime, Space.World);
		}
	}
}
