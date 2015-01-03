using UnityEngine;
using System.Collections;

public class Lecture01 : MonoBehaviour {
	Vector3 vector1;
	Vector3 vector2;
	public Transform target;

	// Use this for initialization
	void Start () {
		vector1 = new Vector3(1, 2, 0);
		vector2 = new Vector3(3, 4, 0);
		float mag = vector1.magnitude;
		Vector3 normalized_vector1 = vector1.normalized;
		vector1.Normalize();
		Vector3 sum = vector1 + vector2;
		Vector3 scalar = vector1 * 3;
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null) {
			return;
		}
		// Vectors can also be used as positions
		// Distance between this object and the target:
		Vector3 distanceVector = target.position - transform.position;
		float distance = distanceVector.magnitude; // I'm expecting to get 5
		// Debug.Log (distance);

		// Let's assume that the cube position gives the direction of the vector
		// and let us calculate the angle between these two vectors.
		float angle = Vector3.Angle(transform.position, target.position);
		float dotProduct = Vector3.Dot(transform.position, target.position);
		Debug.Log ("Dot product: " + dotProduct + ", angle: " + angle);

		// How about the normal of this plane? 
		// Let's find the normal of this plane and draw it.
		Vector3 normalOfPlane = Vector3.Cross(transform.position, target.position); 
		Debug.DrawLine(new Vector3(0,0,0), normalOfPlane.normalized*3);
	}
}
