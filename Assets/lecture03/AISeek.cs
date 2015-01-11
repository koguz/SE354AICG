using UnityEngine;
using System.Collections;

public class AISeek : MonoBehaviour {
	public Vector3 target; 
	private Vector3 velocity;
	public float maxSpeed = 10;
	public float maxAcceleration = 5;
	
	// Use this for initialization
	void Start () {
		target = transform.position;
		velocity = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target - transform.position;
		if( direction.magnitude == 0) {
			/* if we are at the target, then the
			 * magnitude must be zero. There is no need
			 * to run this script, we simply return.
			 */
			return;
		}
		
		/* Let's create the acceleration. */
		Vector3 acceleration = direction.normalized * maxAcceleration;
		/* Using the acceleration, let's update the velocity */
		velocity += acceleration * Time.deltaTime;
		/* What if we have exceeded the maximum speed? */
		if(velocity.magnitude > maxSpeed) {
			velocity = velocity.normalized * maxSpeed;
		}
		
		/* Now, let's do a translation: */
		/* Notice, this time, that we have included Time.deltaTime twice:
		 * once when calculating the velocity and once again here. */
		transform.Translate(velocity*Time.deltaTime, Space.World);
	}

	public Vector3 getVelocity() {
		return velocity;
	}
}
