using UnityEngine;
using System.Collections;

public class FiringSolution : MonoBehaviour {

	public Vector3 firingVector;
	public int firePower = 20;
	public Transform target; 
	// Use this for initialization
	void Start () {
		firingVector = new Vector3(1, 1, 0);
		firingVector.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null) return;
		Debug.DrawLine(transform.position, 5*firingVector);
		if(Input.GetKeyUp(KeyCode.Space)) {
			Fire ();
		}

		if(Input.GetKeyUp(KeyCode.LeftControl)) {
			firingVector = cfs (transform.position, target.transform.position, firePower, Physics.gravity);
			firingVector.Normalize();
		}
	}

	void Fire() {
		firingVector.Normalize();
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		sphere.AddComponent<Rigidbody>();
		sphere.rigidbody.drag = 0;
		// sphere.rigidbody.mass = 3f;
		//sphere.rigidbody.AddForce(firingVector*firePower);
		sphere.rigidbody.velocity = firingVector * firePower;
	}

	Vector3 cfs (Vector3 start, Vector3 end, float muzzle_v, Vector3 gravity)
	{
		Vector3 delta = end - start; // start - end;
		float a = Vector3.Dot(gravity, gravity);
		float b = -4 * (Vector3.Dot(gravity, delta) +
		                (muzzle_v * muzzle_v));
		float c = 4 * Vector3.Dot(delta, delta);
		if (a * c * 4 > b * b) return Vector3.zero;
		float t0 = Mathf.Sqrt((-b + Mathf.Sqrt((b * b) - 4 * (a * c))) / (a * 2));
		float t1 = Mathf.Sqrt((-b - Mathf.Sqrt((b * b) - 4 * (a * c))) / (a * 2));
		
		float ttt = 0;
		if (t0 < 0)
		{
			if (t1 < 0) return Vector3.zero;
			else ttt = t1;
		}
		else
		{
			if (t1 < 0) ttt = t0;
			else ttt = Mathf.Min(t0, t1);
		}
		return ((2 * delta) - (gravity * (ttt * ttt))) / (2 * muzzle_v * ttt);
	}
}
