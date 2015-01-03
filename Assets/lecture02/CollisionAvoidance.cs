using UnityEngine;
using System.Collections;

public class CollisionAvoidance : MonoBehaviour {

	private DynamicSeek seek;
	private DynamicAlign align;
	public Vector3 target = new Vector3(0, 0, 10);
	private int width = 1; // We know that width of cube is 1. 

	// Use this for initialization
	void Start () {
		seek = GetComponent<DynamicSeek>();
		align= GetComponent<DynamicAlign>();
	}
	
	// Update is called once per frame
	void Update () {
		/* We have an original target, but as we move along we 
		 * will avoid obstacles. 
		 * To do so, we will "evade" the obstacles by temporarily
		 * changing the target position.
		 * */
		RaycastHit hit;
		/* Check as far as twice our speed. */
		float distanceToCheck = seek.velocity.magnitude * 2;

		Vector3 tempTarget = new Vector3(0, 0, 0);
		if(Physics.SphereCast(transform.position, width, transform.forward, out hit, distanceToCheck)) {
			// Debug.Log ("hit!");
			/* Now that we know there is something ahead. */
			tempTarget = (distanceToCheck/hit.distance) * (transform.position - hit.transform.position);
		}

		seek.target = target + tempTarget;
		align.target = Mathf.Atan2 (seek.velocity.x, seek.velocity.z) * Mathf.Rad2Deg;
	}
}
