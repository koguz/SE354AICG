using UnityEngine;
using System.Collections;

public class MidpointDisplacement : MonoBehaviour {

	private TerrainData myTData;
	private float[,] data; 
	private int dHeight;
	public bool UseGaussianSmoothing = true;

	// Use this for initialization
	void Start () {
		myTData = Terrain.activeTerrain.terrainData;
		int h = myTData.heightmapHeight;
		int w = myTData.heightmapWidth;

		dHeight = h;

		data = myTData.GetHeights (0, 0, w, h);

		/* The Midpoint Displacement Algorithm - 
		 * page 505
		 * Start with a rectangle ABCD - that is our TerrainData. 
		 * Start with a rectangle ABCD seeded with height values at the four corners. 
		 * Calculate the height at the midpoint E by averaging the values at 
		 * A, B, C, D and adding a random value between -dHeight/2 and dHeight/2. 
		 * 
		 * E = (A+B+C+D)/4 + random(-dHeight/2 + dHeight/2);
		 * 
		 * Multiply dHeight by 2^(-r) and repeat for the sub-squares, until 
		 * sufficient level of detail is reached. 
		 * The magic value for r is 1 -> which makes it 1/2. 
		 * Let's try it. 
		 */ 

		// seed

		data [0, 0] = Random.value;
		data [0, w - 1] = Random.value;
		data [h - 1, 0] = Random.value;
		data [w - 1, h - 1] = Random.value;

		// it makes sense to make it a recursive function. 
		Midpoint(0, 0, w-1, h-1, dHeight);

		myTData.SetHeights (0, 0, data);

		if (UseGaussianSmoothing) {
			GaussianSmooth s = new GaussianSmooth ();
			s.Smooth (myTData);
		}
	}

	private void Midpoint(int x, int y, int w, int h, int H) {
		// In a recursive function, first make sure you have the
		// exit condition in place. 
		if(w < 4) return; // let the smallest square be 10x10. 
		// write down four corners 
		// x,y
		// x+w, y
		// x, y+h
		// x+w, y+h
		float ev = ((float)(data [x, y] + data [x + w, y] + data [x, y + h] + data [x + w, y + h]) / 4.0f)
		           + (float)(Random.Range (-H / 2, H / 2)) / (float)dHeight;

		data [(x+w) / 2, (y+h) / 2] = ev; 
		H = H / 2;

		// now we will have four squares to recurse
		// all have widths and heights as w/2, h/2
		// starting points the same as above, but with new widths and heights. 
		w = w/2; h=h/2;
		Midpoint (x, y, w, h, H);
		Midpoint (x + w, y, w, h, H);
		Midpoint (x, y + h, w, h, H);
		Midpoint (x + w, y + h, w, h, H);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
