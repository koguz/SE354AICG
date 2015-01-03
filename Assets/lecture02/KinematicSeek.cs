using UnityEngine;
using System.Collections;

public class KinematicSeek : MonoBehaviour {

	public Vector3 target; 
	public float maxSpeed = 25;

	// Use this for initialization
	void Start () {
		target = transform.position;
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

		/* Let's create a velocity:
		 * First we normalize the direction. When normalized
		 * the magnitude becomes 1. Then, we multiply that
		 * vector with maxSpeed. 
		 */
		Vector3 velocity = direction.normalized * maxSpeed;

		/* First let's rotate: remember? */
		float a = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, a, 0);

		/* Now, let's do a translation: */
		// transform.position += velocity * Time.deltaTime;
		transform.Translate(velocity*Time.deltaTime, Space.World);
	}
}
