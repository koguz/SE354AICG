using UnityEngine;
using System.Collections;

public class KinematicArrive : MonoBehaviour {

	public Vector3 target; 
	public float maxSpeed = 25;
	public float radius = 0.1f;
	public float timeToTarget = 0.25f;
	
	// Use this for initialization
	void Start () {
		target = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target - transform.position;
		if( direction.magnitude < radius) {
			/* The stopping condition has changed. If we are
			 * within the satisfaction radius, then stop
			 * updating the script
			 */
			return;
		}

		/* Let's create the velocity. We divide the 
		 * direction with timeToTarget. */
		Vector3 velocity = direction / timeToTarget;

		/* The magnitude of velocity can be more than
		 * maxSpeed. Let's check and update if necessary. */
		if(velocity.magnitude > maxSpeed) {
			velocity = velocity.normalized * maxSpeed;
		}
		
		/* First let's rotate: remember? */
		float a = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, a, 0);
		
		/* Now, let's do a translation: */
		// transform.position += velocity * Time.deltaTime;
		transform.Translate(velocity*Time.deltaTime, Space.World);
	}
}
