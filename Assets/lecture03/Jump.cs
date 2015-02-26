using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	private float verticalVelocity = 0.0f;
	public float maxVelocity = 0.5f;
	public float gravity = 0.98f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) DoJump();
		if(transform.position.y < 0) {
			float delta = transform.position.y;
			transform.Translate(new Vector3(0, -delta, 0));
			verticalVelocity = 0.0f;
		}
		if(verticalVelocity != 0) {
			verticalVelocity += 1.5f*(-gravity)*Time.deltaTime;
		}
		transform.Translate(new Vector3(0, verticalVelocity*Time.deltaTime, 0));
	}

	private void DoJump() {
		if(transform.position.y > 0) return;
		verticalVelocity = maxVelocity;
	}
}
