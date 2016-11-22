using UnityEngine;
using System.Collections;

public class GaussianSmooth {

	public int gaussianFilterSize = 3;
	public float gaussianSigma = 1.1f;

	private float gaussian (float sigma, int x, int y)
	{
		float sq = sigma * sigma;
		return 1 / (2 * Mathf.PI * sq) * Mathf.Exp (-1 * (x * x + y * y) / (2 * sq));
	}

	private float[,] getGaussianFilter (int size, float sigma)
	{
		if (size % 2 == 0) {
			Debug.LogError ("GAUSSIAN FILTER SIZE ERROR");
			return null;
		}

		float[,] filter = new float[size, size];
		int s = (size - 1) / 2;
		for (int i = 0; i < size; i++) {
			int px = -s + i;
			for (int j = 0; j < size; j++) {
				int py = -s + j;
				filter [i, j] = gaussian (sigma, px, py);
			}
		}
		return filter;
	}

	public void Smooth (TerrainData myTData) {
		int y = myTData.heightmapHeight;
		int x = myTData.heightmapWidth;
		float[,] data = myTData.GetHeights (0, 0, x, y);
		float[,] temp = myTData.GetHeights (0, 0, x, y);

		// Smooth using the Gaussian Filter... 
		// Default size 3, sigma = 0.5f

		float[,] filter = getGaussianFilter (gaussianFilterSize, gaussianSigma);

		int s = (gaussianFilterSize - 1) / 2;

		for (int i = 0; i < y; i++) {
			for (int j = 0; j < x; j++) {
				float val = 0.0f; // add to this value. 
				for (int fi = 0; fi < gaussianFilterSize; fi++) {
					int py = i - s + fi; 	
					if (py < 0 || py >= y)
						continue; 
					for(int fj = 0; fj < gaussianFilterSize; fj++) {
						int px = j - s + fj; 
						if (px < 0 || px >= x)
							continue;
						val += filter [fi, fj] * temp [px, py];
					}
				}
				data [i, j] = val; 
			}
		}
		myTData.SetHeights (0,0,data);
	}
}
