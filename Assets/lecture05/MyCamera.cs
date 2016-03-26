using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {
	public Transform theCamera;
    public Vector3 relativeVector = new Vector3(0, -2, 3);
    private float rSmoothing = 3.0f;
    private float distanceConstant = 1.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (theCamera == null) return;
        float wrangle = transform.eulerAngles.y;
		float crangle = theCamera.eulerAngles.y;

        crangle = Mathf.LerpAngle(crangle, wrangle, rSmoothing * Time.deltaTime);
        Quaternion cRotate = Quaternion.Euler(0, crangle, 0);
		theCamera.position = transform.position;
		theCamera.position -= cRotate * relativeVector * distanceConstant;
		theCamera.LookAt(transform);
	}
}
