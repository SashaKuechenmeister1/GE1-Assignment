using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {NoiseMap, ColourMap, Mesh};
    public DrawMode drawMode;

	int i;
	int x;
	int y;

	public const int mapChunkSize = 241; // map height & width
	[Range(0,6)]
	public int levelOfDetail;

	public float noiseScale; // number that determines at what distance to have perlin noise
	public int octaves; // number that determines the levels of detail
	[Range(0,1)] // changes persistance into a slider
	public float persistance; // number that determines how much detail is added or removed at each octave (adjusts frequency)
	public float lacunarity; // number that determines how much each octave contributes to the overall shape (adjusts amplitude)

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate; // saves last changed configuration

	public TerrainType[] regions; // allows different terrain types

	// generate the map
	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        
		// sets colours to designated range (e.g. 0 -> 0.5 = water, 0.5 -> 1 = grass)
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (y = 0; y < mapChunkSize; y++) {
            for (x = 0; x <mapChunkSize; x++) {
                float currentHeight = noiseMap[x,y];
                for (i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colourMap[y * mapChunkSize + x] = regions[i].colour; // sets colour to regions range (e.g. 0 -> 0.5 = blue (water))
                        break;
                    }
                }
            }
        }

		// allows user to switch between viewing NoiseMap and ColourMap in Unity
		MapDisplay display = FindObjectOfType<MapDisplay> ();
        if (drawMode == DrawMode.NoiseMap) {
            display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap) {
            display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
		else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		}
		
	}

	void OnValidate() {
		// lacunarity cannot be less than 1
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		// octaves cannot be less than 0
		if (octaves < 0) {
			octaves = 0;
		}
	}
}

// Allows for different terrain types, heights and colours
[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}