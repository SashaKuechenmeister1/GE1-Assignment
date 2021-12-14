using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using Random=UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

	// enum which holds the different run modes: (2D noise map, 2D colour map, 3D Mesh, 3D Falloff map)
    public enum DrawMode {NoiseMap, ColourMap, Mesh, FalloffMap};
    public DrawMode drawMode; // current mode the component is in

	public Noise.NormalizeMode normalizeMode;

	// used for loops
	int i;
	int x;
	int y;

	public const int mapChunkSize = 241; // map height & width
	[Range(0,6)]
	public int editorPreviewLOD; // Level of detail preview

	public float noiseScale; // number that determines at what distance to have perlin noise
	public int octaves; // number that determines the levels of detail
	[Range(0,1)] // changes persistance into a slider
	public float persistance; // number that determines how much detail is added or removed at each octave (adjusts frequency)
	public float lacunarity; // number that determines how much each octave contributes to the overall shape (adjusts amplitude)

	public int seed;
	public Vector2 offset; // vector for offsetting the seed

	public bool useFalloff; // option to use falloff map

	public float meshHeightMultiplier; // how much the Y-axis of the mesh is multiplied by
	public AnimationCurve meshHeightCurve; // curve that allows for customization of the height multiplication

	public bool autoUpdate; // saves last changed configuration


	public TerrainType[] regions; // allows different terrain types

	float[,] falloffMap; // used to create an island rather than a square landmass

	// queue for mapdata
	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	// queue for meshdata
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	void Awake() {
		// generates the fall off map (island)
		falloffMap = FallOffGenerator.GenerateFalloffMap(mapChunkSize);
	}

	public void DrawMapInEditor() {
		MapData mapData = GenerateMapData(Vector2.zero);
		// allows user to switch between viewing NoiseMap, ColourMap, Mesh and FalloffMap in Unity
		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (mapData.heightMap));
		}
		else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (mapData.colourMap, mapChunkSize, mapChunkSize));
		}
		else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD), TextureGenerator.TextureFromColourMap (mapData.colourMap, mapChunkSize, mapChunkSize));
		}
		else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FallOffGenerator.GenerateFalloffMap(mapChunkSize)));
		}
	}

	// requests map data
	public void RequestMapData(Vector2 centre, Action<MapData> callback) {
		// represents the mapdata thread with the callback parameter
		ThreadStart threadStart = delegate {
			MapDataThread(centre, callback);
		};
		// starts new thread
		new Thread(threadStart).Start ();
	}

	void MapDataThread(Vector2 centre, Action<MapData> callback) {
		// passes in the generated map data
		MapData mapData = GenerateMapData (centre);
		// when one thread reaches this point while its executing this, no other thread can execute it as well, it will have to wait its turn.
		lock (mapDataThreadInfoQueue) { 
			// adds a new map thread info of type mapdata
			mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData> (callback, mapData));
		}
	}

	// request mesh data
	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
    	ThreadStart threadStart = delegate {
        	MeshDataThread(mapData, lod, callback);
    	};
    	new Thread(threadStart).Start();
	}

	void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
		// passes in the generated terrain mesh
		MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod); 
		// when one thread reaches this point while its executing this code, no other thread can execute it as well, it will have to wait its turn.
		lock (meshDataThreadInfoQueue) { 
			// adds a new map thread info of type meshdata
			meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData> (callback, meshData));
		}
	} 


	void Update() {
		if (mapDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}
	}

	// generate the map
	MapData GenerateMapData(Vector2 centre) {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);
        
		// sets colours to designated range (e.g. 0 -> 0.5 = water, 0.5 -> 1 = grass)
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				// if falloff map is used
				if (useFalloff) {
					noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y] - falloffMap[x,y]); 
				}
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight >= regions [i].height) {
						colourMap [y * mapChunkSize + x] = regions [i].colour;
					}
					else {
						break;
					}
				}
			}
		}

		return new MapData (noiseMap, colourMap);
		
	}

void OnValidate() {
	// if lacunarity less than 1, set to 1
	if (lacunarity < 1) {
		lacunarity = 1;
	}
	// if octaves less than 0, set to 0
	if (octaves < 0 ) {
		octaves = 0;
	}
	
	falloffMap = FallOffGenerator.GenerateFalloffMap(mapChunkSize);
}	

	// handles both mapdata and meshdata
	struct MapThreadInfo<T> {
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo (Action<T> callback, T parameter) {
			this.callback = callback;
			this.parameter = parameter;
		}

	}

}




// Allows for different terrain types, heights and colours
[System.Serializable]
public struct TerrainType {
	// Name of region
    public string name;
	// The cutoff height of region
    public float height;
	// Colour of region
    public Color colour;
}

public struct MapData {
	public readonly float[,] heightMap;
	public readonly Color[] colourMap;

	public MapData (float[,] heightMap, Color[] colourMap) {
		this.heightMap = heightMap;
		this.colourMap = colourMap;
	}
}