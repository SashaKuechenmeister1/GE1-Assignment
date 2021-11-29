using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {NoiseMap, ColourMap, Mesh, FalloffMap};
    public DrawMode drawMode;

	public Noise.NormalizeMode normalizeMode;

	int i;
	int x;
	int y;

	public const int mapChunkSize = 241; // map height & width
	[Range(0,6)]
	public int editorPreviewLOD;

	public float noiseScale; // number that determines at what distance to have perlin noise
	public int octaves; // number that determines the levels of detail
	[Range(0,1)] // changes persistance into a slider
	public float persistance; // number that determines how much detail is added or removed at each octave (adjusts frequency)
	public float lacunarity; // number that determines how much each octave contributes to the overall shape (adjusts amplitude)

	public int seed;
	public Vector2 offset;

	public bool useFalloff;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate; // saves last changed configuration

	public TerrainType[] regions; // allows different terrain types

	float[,] falloffMap;

	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	void Awake() {
		falloffMap = FallOffGenerator.GenerateFalloffMap(mapChunkSize);
	}

	public void DrawMapInEditor() {
		MapData mapData = GenerateMapData(Vector2.zero);
		// allows user to switch between viewing NoiseMap and ColourMap in Unity
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

	public void RequestMapData(Vector2 centre, Action<MapData> callback) {
		ThreadStart threadStart = delegate {
			MapDataThread(centre, callback);
		};
		new Thread(threadStart).Start ();
	}

	void MapDataThread(Vector2 centre, Action<MapData> callback) {
		MapData mapData = GenerateMapData (centre);
		lock (mapDataThreadInfoQueue) { //when one thread reaches this point while its executing this code, no other thread can execute it aswell, it will have to wait its turn.
			mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData> (callback, mapData));
		}
	}

	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
    	ThreadStart threadStart = delegate {
        	MeshDataThread(mapData, lod, callback);
    	};
    	new Thread(threadStart).Start();
	}

	void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
		MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod); //
		lock (meshDataThreadInfoQueue) { //when one thread reaches this point while its executing this code, no other thread can execute it aswell, it will have to wait its turn.
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
		// lacunarity cannot be less than 1
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		// octaves cannot be less than 0
		if (octaves < 0) {
			octaves = 0;
		}
		falloffMap = FallOffGenerator.GenerateFalloffMap(mapChunkSize);
	}

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
    public string name;
    public float height;
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