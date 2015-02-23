using UnityEngine;
using System.Collections;

public class AddForce : MonoBehaviour {
	public Vector3 target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space)) {
			Vector3 temp = new Vector3(Random.Range(0,10), Random.Range (0,10), Random.Range(0, 10));
			Debug.Log (temp);
			rigidbody.AddForce(temp * 100);
		}
	}
}
