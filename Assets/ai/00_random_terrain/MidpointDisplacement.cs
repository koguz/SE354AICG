using UnityEngine;
using System.Collections;

public class MidpointDisplacement : MonoBehaviour
{

	private TerrainData myTData;
	private float[,] data;
	private int dHeight;
	public bool UseGaussianSmoothing = true;
	public bool NormaliseTerrain = true;
	public int NormaliseOneOver = 3;

	// Use this for initialization
	void Start ()
	{
		myTData = Terrain.activeTerrain.terrainData;
		int h = myTData.heightmapHeight;
		int w = myTData.heightmapWidth;

		dHeight = h;
		int H = dHeight;

		data = myTData.GetHeights (0, 0, w, h);

		/* The Midpoint Displacement Algorithm - 
		 * page 505
		 * Start with a rectangle ABCD - that is our TerrainData. 
		 * Start with a rectangle ABCD seeded with height values at the four corners. 
		 * Calculate the height at the midpoint E by averaging the values at 
		 * A, B, C, D and adding a random value between -dHeight/2 and dHeight/2. 
		 * This is the diamond step. 
		 * 
		 * E = (A+B+C+D)/4 + random(-dHeight/2 + dHeight/2);
		 * 
		 * Then, update the top center, bottom center, left middle and right middle
		 * points. This is called the square step. 
		 * 
		 * The procedure is shown in the figure under its Wikipedia article:
		 * https://en.wikipedia.org/wiki/Diamond-square_algorithm 
		 * 
		 * Multiply dHeight by 2^(-r) and repeat for the sub-squares, until 
		 * sufficient level of detail is reached. 
		 * The magic value for r is 1 -> which makes it 1/2. 
		 * Let's try it. 
		 */ 

		// seed

		data [0, 0] = getRandomInitialValue ();
		data [0, w - 1] = getRandomInitialValue ();
		data [h - 1, 0] = getRandomInitialValue ();
		data [w - 1, h - 1] = getRandomInitialValue ();

		int ws = w-1; 
		while (ws > 1) {
			// how many squares do we have at this iteration? 
			int ss = (w-1) / ws; 
			for (int li = 0; li < ss; li++) {
				for (int lj = 0; lj < ss; lj++) {
					int px = li * ws; 
					int py = lj * ws;
					int pw = ws;
					int ph = ws;
					// diamond step first
					float mid = ((float)(data [px, py] + data [px + pw, py] + data [px, py + ph] + data [px + pw, py + ph]) / 4.0f) + getRandomH (H);
					data [px + (pw / 2), py + (ph / 2)] = mid;
					// now the square step
					// top center
					data [px + (pw / 2), py] = ((float)(data [px, py] + data [px + pw, py] + mid) / 3.0f) + getRandomH (H);
					// bottom center
					data [px + (pw / 2), py + ph] =((float)(data [px, py + ph] + data [px, py] + mid) / 3.0f) + getRandomH (H);
					// left center
					data [px, py + (ph / 2)] = ((float)(data [px, py] + data [px, py + ph] + mid) / 3.0f) + getRandomH (H);
					// right center
					data [px + pw, py + (ph / 2)] = ((float)(data [px + pw, py + ph] + data [px + pw, py] + mid) / 3.0f) + getRandomH (H);
				}
			}
			H = H / 2;
			ws = ws / 2;
		}

		//this is normalization 
		if (NormaliseTerrain) {
			float minv = 1.0f;
			float maxv = 0.0f;
			for (int i = 0; i < h; i++) {
				for (int j = 0; j < w; j++) {
					if (data [i, j] > maxv)
						maxv = data [i, j];
					else if (data [i, j] < minv) {
						minv = data [i, j];
					}
				}
			}
			if (minv <= 0 || maxv >= 1.0f) {
				// then we need to normalize.
				// otherwise, keep it as it is...
				float range = maxv - minv; 
				for (int i = 0; i < h; i++) {
					for (int j = 0; j < w; j++) {
						data [i, j] -= minv;
						data [i, j] = data [i, j] / (NormaliseOneOver*range);
					}
				}
			}
		}
				
		
		myTData.SetHeights (0, 0, data);

		if (UseGaussianSmoothing) {
			GaussianSmooth s = new GaussianSmooth ();
			s.Smooth (myTData);
		}
	}

	private float getRandomH (int H)
	{
		return (float)(Random.Range (-H / 4, H / 4)) / (float)dHeight;
	}

	private float getRandomInitialValue ()
	{
		float r = Random.value;
		if (r <= 0.25)
			r = 0.02f;
		else if (r <= 0.5)
			r = 0.04f;
		else if (r <= 0.75)
			r = 0.06f;
		else
			r = 0.08f;
		return r;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
