using UnityEngine;
using System.Collections;

public class Separation : MonoBehaviour {

	private ArrayList entities;
	public float decayCoefficient = 0.1f;
	public float threshold = 3.0f;
	public float maxAcceleration = 1.0f;
	private DynamicSeek seek;
	
	// Use this for initialization
	void Start () {
		seek = GetComponent<DynamicSeek>();
		entities = new ArrayList();
		foreach (Transform t in GameObject.FindObjectsOfType(typeof(Transform)))
		{
			if (t.name == "Entity" && t != transform)
			{
				entities.Add(t);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 target = Vector3.zero;
		foreach (Transform t in entities)
		{
			Vector3 direction = transform.position - t.position;
			float distance = direction.magnitude;
			if (distance < threshold)
			{
				float strength = Mathf.Min(decayCoefficient / (distance * distance),
				                           maxAcceleration);
				direction.Normalize();
				target += direction * strength;
			}
		}
		seek.target = transform.position + target;
	}
}
