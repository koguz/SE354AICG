using UnityEngine;
using System.Collections;

public class FaultFormation : MonoBehaviour {

	private TerrainData myTData;
	public bool UseGaussianSmoothing = true;

	// Use this for initialization
	void Start () {
		myTData = Terrain.activeTerrain.terrainData;
		int y = myTData.heightmapHeight;
		int x = myTData.heightmapWidth;

		float[,] data = myTData.GetHeights (0, 0, x, y);

		// Fractal Terrain Generation: Fault Formation.
		/* Briefly, choose two random points. Assume that 
		 * there is a vector from p1 to p2. Then, for each 
		 * point on the map, find if the point is on the left, 
		 * or right hand side of this vector. Add an offset
		 * height value for one side. Pick the side randomly.
		 * Decrement height value at each step. 
		 * The book runs it for 64 iterations. At each iteration
		 * the height is set as 
		 * h_next = h_0 + (i/n)*(h_n - h_0)
		 * In order to determine the side of the line, the book
		 * suggests computing the dot product of the vector p1p2
		 * and p1p0, where p0 is the point we are investigating. 
		 * If the z component > 0, then it is on the left, otherwise
		 * on the right. If == 0, then it is on the line itself. 
		 * Enough talk, let's code. 
		 */

		int N = 64;
		float h_0 = 0.01f;
		float h_n = 0.0025f;
		float h = h_0;
		for (int i = 0; i < N; i++) {
			// pick two random points
			Vector3 p1 = new Vector3(Random.Range(0, x-1), Random.Range(0, y-1), 0);
			Vector3 p2 = new Vector3(Random.Range(0, x-1), Random.Range(0, y-1), 0);
			Vector3 p1p2 = p2 - p1;

			bool left = Random.value < 0.5;

			for (int a = 0; a < x; a++) {
				for (int b = 0; b < y; b++) {
					Vector3 tv = (new Vector3(a, b, 0)) - p1;
					Vector3 r = Vector3.Cross (p1p2, tv);
					if (r.z < 0 && left) {
						data [a, b] += h;
					} else if (r.z > 0 && !left) {
						data [a, b] += h; 
					}
				}
			}
			h = h_0 + ((float)i / (float)N) * (h_n - h_0); 
		}

		// APPLY FIR FILTER for FAULT FORMATION

		float k = 0.9f;
		for (int i = 1; i < y; i++) {
			for (int j = 1; j < x; j++) {
				data [i, j] = k * data [i, j - 1] + (1 - k) * data [i, j];
			}
		}
		for (int i = 1; i < y; i++) {
			for (int j = 1; j < x; j++) {
				data [i, j] = k * data [i - 1, j] + (1 - k) * data [i, j];
			}
		}

		myTData.SetHeights (0, 0, data);

		if (UseGaussianSmoothing) {
			GaussianSmooth s = new GaussianSmooth ();
			s.Smooth (myTData);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
