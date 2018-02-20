using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour {

	public int numberOfBoids = 100; 

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numberOfBoids; i++) {
			GameObject temp = Instantiate (Resources.Load ("Entity") as GameObject,
				new Vector3 (Random.Range (1, 100), 0, Random.Range (1, 100)),
				Quaternion.identity
			);
			temp.transform.Rotate (0, Random.Range (-30, 30), 0);
			temp.name = "ABoid";
			temp.tag = "boid";
			temp.AddComponent<KArrive> ();
			temp.AddComponent<KBoid> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
