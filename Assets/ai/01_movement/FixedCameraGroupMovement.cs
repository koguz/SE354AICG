using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraGroupMovement : MonoBehaviour {

	private ArrayList entities;
	// Use this for initialization
	void Start () {
		entities = new ArrayList ();
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("gr"))
			entities.Add (g);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			RaycastHit h;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out h)) {
				// Debug.Log (h.point);
				// Debug.DrawRay (ray.origin, ray.direction * 100, Color.yellow);
				Vector3 target = h.point;
				target.y = 0.5f; // let's ignore y 
				// Change KArrive to DArrive for dynamic version.
				// GameObject.Find("Entity").GetComponent<KArrive>().target = target;
				int v = -3;
				foreach (GameObject g in entities) {
					g.GetComponent<KArrive> ().target = target + (new Vector3 (0, 0, v));
					v += 2;
				}
			}

		}
	}
}
