using UnityEngine;
using System.Collections;

public static class TextureGenerator {

	// create a texture out of a 2-dimensional colourMap
	public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point; // fixes blur
		texture.wrapMode = TextureWrapMode.Clamp; // fixes map wrapping around to other side
		texture.SetPixels (colourMap);
		texture.Apply ();

		return texture;
	}

	// create a texture out of a 2-dimensional heightMap
	public static Texture2D TextureFromHeightMap(float[,] heightMap) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]); // Linearly interpolates between black and white by heightMap.
			}
		}

		return TextureFromColourMap (colourMap, width, height);

	}

}