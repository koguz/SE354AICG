using UnityEngine;
using System.Collections;

public class DynamicWander : MonoBehaviour {
	private int wanderOffset = 6;
	private int wanderRadius = 10;
	private float wanderOrientation = 0.0f;

	private DynamicSeek seek;
	private DynamicAlign align;

	// Use this for initialization
	void Start () {
		/* I get the components and can access them now 
		 * by variables seek and align. */
		seek = GetComponent<DynamicSeek>();
		align= GetComponent<DynamicAlign>();
	}
	
	// Update is called once per frame
	void Update () {
		/* Take a closer look at this operation: */
		float rndBinomial = Random.value - Random.value;
		/* It generates values between -1 and 1. The values 
		 * are mostly around 0. It is called Binomial Random */

		/* Get a random orientation within the range of wanderRadius */
		wanderOrientation += rndBinomial * wanderRadius;
		float targetOrientation = wanderOrientation + transform.eulerAngles.y;

		/* To get to the target, we start from our own position. 
		 * Then we add wanderOffset*transform.forward to this position. 
		 * This new position is the center of the circle. 
		 */
		Vector3 target = transform.position + wanderOffset * transform.forward;
		/* Now we have to get to the target. From the center of the circle, we 
		 * need to rotate targetOrientation and translate a distance of wanderRadius.
		 */
		target += (Quaternion.Euler(0, targetOrientation, 0)*Vector3.forward) * wanderRadius;

		/* Now delegate target and direction to related behaviours: */
		seek.target = target; 
		/* To find the direction, we use the seek's velocity */
		align.target = Mathf.Atan2 (seek.velocity.x, seek.velocity.z) * Mathf.Rad2Deg;
	}
}
