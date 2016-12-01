using UnityEngine;
using System.Collections;

public class RandomTerrain : MonoBehaviour
{

	private TerrainData myTData;
	public bool UseGaussianSmoothing = true;

	// Use this for initialization
	void Start ()
	{
		myTData = Terrain.activeTerrain.terrainData;
		int y = myTData.heightmapHeight;
		int x = myTData.heightmapWidth;
		float[,] data = new float[y, x];
		float[,] temp = new float[y, x];

		// The book is Game Programming Gems  1
		// This is from page 484-5

		for (int i = 0; i < y; i++) {
			for (int j = 0; j < x; j++) {
				Random.InitState (j + (j * i));
				float r = Random.value;
				if (r <= 0.25)
					r = 0.0f;
				else if (r <= 0.5)
					r = 0.0025f;
				else if (r <= 0.75)
					r = 0.0050f;
				else
					r = 0.0075f;
				data [i, j] = r;
				temp [i, j] = r;
			}
		}

		myTData.SetHeights (0, 0, data);

		// FINAL STEP is SMOOTHING

		// Nevermind the smoothing algorithms below. 
		// Let's try the Gaussian filter - which I believe will
		// perform much better. 

		if (UseGaussianSmoothing) {
			GaussianSmooth s = new GaussianSmooth ();
			s.Smooth (myTData);
		}

		// and I was right. Much better :) 

		// This smoothing algorithm is on page 486
		// Written a little shaky 
		// I had to extract the step, but this causes the borders 
		// to stay unsmoothed. Besides, the pseudo-code has errors;
		// it uses <= for boundaries in the inner loops. 
		// The book suggest another smoothing algorithm on 
		// pages 486-8
		/*
		int step = 6;
		for (int i = 0; i < y-step; i = i + step) {
			for (int j = 0; j < x-step; j = j + step) {
				float total = 0.0f;
				for (int ilocal = i; ilocal < i + step; ilocal++) {
					for (int jlocal = j; jlocal < j + step; jlocal++) {
						total += data [ilocal, jlocal];
					}
				}
				float average = total / (step * step);
				for (int ilocal = i; ilocal < i + step; ilocal++) {
					for (int jlocal = j; jlocal < j + step; jlocal++) {
						data [ilocal, jlocal] = average;
					}
				}
			}
		}
		*/


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
