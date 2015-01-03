using UnityEngine;
using System.Collections;

public class PathFollowing : MonoBehaviour {

	private DynamicAlign align;
	private DynamicSeek seek;
	
	private ArrayList targets;
	private int currentid = 0;
	// Use this for initialization
	void Start () {
		align = GetComponent<DynamicAlign>();
		seek = GetComponent<DynamicSeek>();
		/* a list of targets */
		targets = new ArrayList();
		targets.Add(new Vector3(10, 0, 10));
		targets.Add(new Vector3(0, 0, 20));
		targets.Add(new Vector3(-10, 0, 10));
		targets.Add(new Vector3(-10, 0, 0));
		targets.Add(new Vector3(0, 0, 0));
		
		/* We instantiate cubes at target points, so that we can see the points better. */
		for (int i = 0; i < targets.Count; i++) {
			Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), (Vector3)targets[i], Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		/* At each update get the distance to current target */
		Vector3 distance = transform.position - (Vector3)targets[currentid];
		/* If we are within a satisfactory margin, move on to the next target. */
		if (distance.magnitude < 0.5) currentid = (currentid + 1) % targets.Count;

		/* Pass target to seek and align scripts */
		seek.target = (Vector3) targets[currentid];
		align.target = Mathf.Atan2(seek.velocity.x, seek.velocity.z) * Mathf.Rad2Deg;
	}
}
