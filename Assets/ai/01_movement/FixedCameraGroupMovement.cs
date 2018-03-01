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

				// find the center of gravity
				Vector3 cog = Vector3.zero;
				foreach (GameObject g in entities) {
					cog += g.transform.position;
				}
				cog /= entities.Count;
				cog.y = 0.5f;
				// find the direction
				Vector3 direction = cog - target;
				direction.Normalize ();
				Vector3 positions = Quaternion.Euler (0, 90, 0) * direction;
				positions.Normalize ();


				int v = -3;
				foreach (GameObject g in entities) {
					g.GetComponent<KArrive> ().target = target + v * positions;
					v += 2;
				}
			}

		}
	}
}
