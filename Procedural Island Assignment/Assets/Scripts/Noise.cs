using UnityEngine;
using System.Collections;

public static class Noise {

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
		
		int i;
		int x;
		int y;
		
		float[,] noiseMap = new float[mapWidth,mapHeight];

		// creates a random map configuration if different seed is selected
		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-10000, 10000) + offset.x;
			float offsetY = prng.Next (-10000, 10000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		// prevent error where if value was below 0, program wouldn't work
		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		// loops through noiseMap
		for (y = 0; y < mapHeight; y++) {
			for (x = 0; x < mapWidth; x++) {
		
				float amplitude = 1;
				float frequency = 1; 
				float noiseHeight = 0;

				// when changing noise Scale, focuses to center rather than top-right corner
				for (i = 0; i < octaves; i++) {
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1; // (* 2 - 1) allows for noiseHeight to be negative value
					noiseHeight += perlinValue * amplitude;
					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = noiseHeight;
			}
		}

		for (y = 0; y < mapHeight; y++) {
			for (x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]); /* Calculates the linear parameter noiseMap that
																										  produces the interpolant value within 
																										  the range [minNoiseHeight, maxNoiseHeight] */
			}
		}

		return noiseMap;
	}

}