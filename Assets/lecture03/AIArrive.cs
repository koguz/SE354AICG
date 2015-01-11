using UnityEngine;
using System.Collections;

public class AIArrive : MonoBehaviour {
	public Vector3 target; 
	private Vector3 velocity;
	public float maxSpeed = 10;
	public float slowRadius = 5.0f;
	public float targetRadius = 0.1f;
	public float maxAcceleration = 5;
	public float timeToTarget = 0.25f;
	
	// Use this for initialization
	void Start () {
		target = transform.position;
		velocity = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target - transform.position;
		float   distance  = direction.magnitude;
		if( distance < targetRadius ) {
			/* We are within an acceptable margin of stopping. */
			return;
		}

		/* Let's calculate the target speed. From this we will create
		 * a target velocity. We will use our maxAcceleration to 
		 * get to that velocity. */
		float targetSpeed = 0;
		if(distance > slowRadius) {
			/* We are still out of slowRadius. Pedal to the metal. */
			targetSpeed = maxSpeed;
		} else {
			/* The distance/slowRadius will always be less than 1. */
			targetSpeed = maxSpeed * (distance / slowRadius);
		}
		/* Let's take a look at the targetAcceleration. We find it by subtracting
		 * current velocity from the velocity we find using targetSpeed. 
		 * We also divide by timeToTarget. 
		 * 
		 * This difference gives us the amount of change we should make to 
		 * get this velocity. Of course, if the difference is greater than the
		 * maxAcceleration, we will be limiting it. 
		 */
		Vector3 targetAcceleration = ((direction.normalized * targetSpeed) - velocity)/timeToTarget;
		if(targetAcceleration.magnitude>maxAcceleration) {
			targetAcceleration = targetAcceleration.normalized * maxAcceleration;
		}

		/* Now that we have an acceleration, let's update velocity.
		 * We also check if we exceed maxSpeed. 
		 */
		velocity += targetAcceleration * Time.deltaTime;
		if(velocity.magnitude > maxSpeed) {
			velocity = velocity.normalized * maxSpeed;
		}
		
		/* Now, let's do a translation: */
		// transform.position += velocity * Time.deltaTime;
		transform.Translate(velocity*Time.deltaTime, Space.World);
	}

	public Vector3 getVelocity() {
		return velocity;
	}
}
