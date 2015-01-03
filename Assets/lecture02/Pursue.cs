using UnityEngine;
using System.Collections;

public class Pursue : MonoBehaviour {

	public Transform target;
	private DynamicSeek seek;
	private DynamicAlign align;
	private float maxPrediction = 2.0f;
	// Use this for initialization
	void Start () {
		seek = GetComponent<DynamicSeek>();
		align = GetComponent<DynamicAlign>();
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null) {
			Debug.Log ("No target??");
			return;
		}
		/* In source, we make the prediction proportionate with the distance */
		float distance = (target.position - transform.position).magnitude;
		float speed = seek.velocity.magnitude;
		float prediction = 0.0f;

		/* If the distance is greater than the current speed, 
		 * go with maximum prediction. It is probably a little far away. */
		if(speed <= distance / maxPrediction) {
			prediction = maxPrediction;
		} else {
			prediction = distance / speed;
		}
		/* Now pass the target to the seek script. */
		seek.target = target.position + (target.GetComponent<DynamicSeek>().velocity * prediction);
		/* Pass the direction to align */
		align.target = Mathf.Atan2 (seek.velocity.x, seek.velocity.z) * Mathf.Rad2Deg;
	}
}
