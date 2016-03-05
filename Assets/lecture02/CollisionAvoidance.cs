using UnityEngine;
using System.Collections;

public class CollisionAvoidance : MonoBehaviour {
	
	private DynamicArrive arrive;
	private DynamicAlign align;
	private GameObject k;
	public Vector3 target = new Vector3(0, 0, 10);
	private int width = 1; // We know that width of cube is 1. 

	// Use this for initialization
	void Start () {
		arrive = GetComponent<DynamicArrive>();
		align= GetComponent<DynamicAlign>();
		k = GameObject.CreatePrimitive(PrimitiveType.Cube);
		k.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		// if( (target-transform.position).magnitude < 0.2 ) return;
		/* We have an original target, but as we move along we 
		 * will avoid obstacles. 
		 * To do so, we will "evade" the obstacles by temporarily
		 * changing the target position.
		 * */
		RaycastHit hit;
		/* Check as far as twice our speed. */
		float distanceToCheck = arrive.velocity.magnitude * 2;

		Vector3 tempTarget = new Vector3(0, 0, 0);
		if(Physics.SphereCast(transform.position, width, transform.forward, out hit, distanceToCheck)) {
			// Debug.Log ("hit!");
			/* Now that we know there is something ahead. */
			float power = distanceToCheck / hit.distance; // can go to infinity!
			if(power > 1000) power = 1000;
			Vector3 tempDir = Vector3.zero;
			if (Vector3.Angle (transform.position, hit.transform.position) == 0) {
				if (Random.Range (0, 1) == 0) {
					tempDir = transform.right * 2; // this can be further improved?
				} else
					tempDir = transform.right * -2;
			}
			tempTarget = power * (transform.position - hit.transform.position + tempDir);
		}
		arrive.target = target + tempTarget;
		k.transform.position = target + tempTarget;
		align.target = Mathf.Atan2 (arrive.velocity.x, arrive.velocity.z) * Mathf.Rad2Deg;
	}
}
