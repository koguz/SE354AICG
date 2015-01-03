using UnityEngine;
using System.Collections;

public class Lecture01_2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, Time.deltaTime * 90, 0));
		Quaternion rot = transform.rotation;
		Debug.Log (rot);
	}
}
