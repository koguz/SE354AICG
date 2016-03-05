using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public int[,] alan = new int[30, 30];
	public Material brown;
	public Material gray;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 30; i++)
		{
			for (int j = 0; j < 30; j++)
			{
				alan[i, j] = 0;
			}
		}
		for (int i = 10; i < 20; i++)
		{
			alan[10, i] = 1;
		}
		for (int i = 5; i < 11; i++)
		{
			alan[i, 20] = 1;
		}
		
		GameObject kup = GameObject.CreatePrimitive(PrimitiveType.Cube);
		for (int i = 0; i < 30; i++)
		{
			for (int j = 0; j < 30; j++)
			{
				Vector3 pos = new Vector3(i, -0.56f, j);
				GameObject g = (GameObject)GameObject.Instantiate(
					kup,
					pos,
					Quaternion.identity);
				if (alan[i, j] == 1) {
					g.transform.localScale = new Vector3(0.98f, 0.9f, 0.98f);
					g.GetComponent<Renderer>().material = gray;
				}
				else {
					g.transform.localScale = new Vector3(0.98f, 0.05f, 0.98f);
					g.GetComponent<Renderer>().material = brown;
				}
			}
		}
		GameObject.DestroyObject(kup);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
