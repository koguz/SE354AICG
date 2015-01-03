using UnityEngine;
using System.Collections;

public class LookAtMe : MonoBehaviour {
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null) {
			return;
		}

		Vector3 direction = target.transform.position - transform.position;
		Debug.DrawLine(transform.position, direction);

		// rotate accordingly
		float a = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, a, 0);

		// http://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
		//transform.rotation = Quaternion.LookRotation(direction);
	}
}
