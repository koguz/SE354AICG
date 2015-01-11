using UnityEngine;
using System.Collections;

public class Flocking : MonoBehaviour {
	private AISeek seek;
	private AIAlign align;
	private AIWander wander;
	private AISeparation separation;
	private ArrayList entities;
	public float radius = 15.0f;

	// Use this for initialization
	void Start () {
		seek = gameObject.AddComponent ("AISeek") as AISeek;
		align = gameObject.AddComponent("AIAlign") as AIAlign;
		wander = gameObject.AddComponent("AIWander") as AIWander;
		separation = gameObject.AddComponent("AISeparation") as AISeparation;

		separation.threshold = 2.0f;
		separation.maxAcceleration = 20;

		seek.maxSpeed = Random.Range(7,10);

		entities = new ArrayList();
		foreach(Transform t in GameObject.FindObjectsOfType(typeof(Transform))) {
			if(t.name == "Entity" && t != transform) entities.Add(t);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 target = Vector3.zero;
		Vector3 centerOfMass = Vector3.zero;
		Vector3 flockVelocity = Vector3.zero;

		int count = 0;
		foreach(Transform t in entities) {
			if( (transform.position-t.position).magnitude < radius ) {
				count++;
				centerOfMass += t.position;
				flockVelocity += t.GetComponent<AISeek>().getVelocity();
			}
		}

		if(count == 0) {
			return;
		}

		centerOfMass /= count;
		flockVelocity /= count;

		target = wander.getTarget() + separation.getTarget() + 4*centerOfMass;
		seek.target = target + flockVelocity * Time.deltaTime;
		align.setTargetToVector(seek.target);

	}
}
