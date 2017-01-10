using UnityEngine;
using System.Collections;

public class GaussianSmooth {

	public int gaussianFilterSize = 3;
	public float gaussianSigma = 1.1f;

	public GaussianSmooth(int filterSize = 3, float sigma = 1.1f) {
		gaussianFilterSize = filterSize;
		gaussianSigma = sigma;
	}

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

	//public float[,] Smooth (TerrainData myTData) {
	public float[,] Smooth (float[,] data) {
		int x = data.GetLength (0);
		int y = data.GetLength (1);
		float[,] temp = new float[x,y]; 
		for (int i = 0; i < x; i++)
			for (int j = 0; j < y; j++)
				temp [i, j] = data [i, j];

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
						val += filter [fi, fj] * temp [py, px];
					}
				}
				data [i, j] = val; 
			}
		}
		return data; // myTData.SetHeights (0,0,data);
	}
}
