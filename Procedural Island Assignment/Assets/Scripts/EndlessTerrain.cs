using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour {

	// size of terrain generated 
	const float scale = 5f;
	// threshold which makes viewer move before updating the chunks
	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	// array containing level of detail info
	public LODInfo[] detailLevels;

	// how far the viewer can see
	public static float maxViewDst;

	public Transform viewer;

	public Material mapMaterial;

	// viewer position
	public static Vector2 viewerPosition;
	// viewers old position
	Vector2 viewerPositionOld;

	static MapGenerator mapGenerator;

	int chunkSize;
	// checks how many chunks are visible based on the chunk size and max view distance
	int chunksVisibleInViewDst;

	// dictionary of all the cooridnates and terrain chunks to prevent duplicates
	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();

	// list of terrain chunks
	static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();


	void Start() {
		mapGenerator = FindObjectOfType<MapGenerator> ();

		// max view distance is now equal to the last element of detail levels
		maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
		chunkSize = MapGenerator.mapChunkSize - 1;
		// number of chunks visible in the view distance = how many times the chunk size can be divided into the view distance
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

		UpdateVisibleChunks ();
	}

	// updates viewer position every frame
	void Update() {
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / scale;

		// if true, updates visible chunks
		if ((viewerPositionOld-viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();
		}
	}
		
	void UpdateVisibleChunks() {
		// loops through all the terrain chunks visible in the last update and sets them to invisible
		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
			terrainChunksVisibleLastUpdate [i].SetVisible (false);
		}
		// clears the list
		terrainChunksVisibleLastUpdate.Clear ();
			
		// gets coordinate X and Y of where the character is standing on
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / chunkSize);
		// loops through all the surrounding chunks
		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
					// updates an already generated terrain chunk
					terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					if (terrainChunkDictionary [viewedChunkCoord].IsVisible()) {
						terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
					}

				}
				else {
					// instantiates a new terrain chunk
					terrainChunkDictionary.Add (viewedChunkCoord, new TerrainChunk (viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
				}

			}
		}
	}

	// class to represent the terrain chunk object
	public class TerrainChunk {

		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		MeshCollider meshCollider;

		// array for detail levels
		LODInfo[] detailLevels;
		// array for level of detail meshes
		LODMesh[] lodMeshes;

		// stores the received mapdata
		MapData mapData;
		// checks if map data received or not
		bool mapDataReceived;

		int previousLODIndex = -1;

		// terrain chunk constructor
		public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material) {
			this.detailLevels = detailLevels;

			position = coord * size;
			// used to find the point on perimeter closest to another point
			bounds = new Bounds(position,Vector2.one * size);
			// creates the position in a 3D space
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = new GameObject("Terrain Chunk");
			// adds mesh renderer component
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			// adds mesh filter component
			meshFilter = meshObject.AddComponent<MeshFilter>();
			// adds mesh collider component
			meshCollider = meshObject.AddComponent<MeshCollider>();
			meshRenderer.material = material;

			meshObject.transform.position = positionV3 * scale;
			meshObject.transform.parent = parent;
			meshObject.transform.localScale = Vector3.one * scale;

			// default state of terrain chunk is invisible, to let the update method determine whether it should become visible
			SetVisible(false);

			lodMeshes = new LODMesh[detailLevels.Length];
			// create lod meshes
			for (int i = 0; i < detailLevels.Length; i++) {
				lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
			}
			// request map data from mapgenerator
			mapGenerator.RequestMapData(position, OnMapDataReceived);
		}

		void OnMapDataReceived(MapData mapData) {
			this.mapData = mapData;
			mapDataReceived = true;

			// creates texture
			Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
			meshRenderer.material.mainTexture = texture;

			UpdateTerrainChunk();
		}


		/* tells the terrain chunk to update itself (find the point i its perimeter that is the closest to viewer position 
		 and find distance between that point and viewer, if  distance is less than maximum view distance, then will make
		 sure mesh obj is enabled. if exceeds max view distance will disable) */
		public void UpdateTerrainChunk() {
			// only runs if map data has been received
			if (mapDataReceived) {
				// finds the viewer distance from the nearest edge
				float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance (viewerPosition));
				// visibility determined by viewer distance from edge begin equal or less than the maximum view distance
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				/* looks at the distance of the viewer from the nearest edge and compares it with the distance
				threshold of each of the detail levels to determine which one should be displayed */
				if (visible) {
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++) {
						if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold) {
							lodIndex = i + 1;
						}
						else {
							break;
						}
					}

					if (lodIndex != previousLODIndex) {
						LODMesh lodMesh = lodMeshes[lodIndex];
						if (lodMesh.hasMesh) {
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
							meshCollider.sharedMesh = lodMesh.mesh;
						}
						else if (!lodMesh.hasRequestedMesh) {
							lodMesh.RequestMesh(mapData);
						}
					}
					terrainChunksVisibleLastUpdate.Add (this);
				}

				SetVisible (visible);
			}
		}

		// method sets the mesh object to visible if true
		public void SetVisible(bool visible) {
			meshObject.SetActive (visible);
		}

		// method finds out if the mesh object is visible
		public bool IsVisible() {
			return meshObject.activeSelf;
		}

	}

	// class which fetches its own mesh from the MapGenerator
	class LODMesh {
		public Mesh mesh;
		// checks whether or not the mesh has been requested
		public bool hasRequestedMesh;
		// checks if mesh has been received
		public bool hasMesh;
		// level of detail of this current mesh
		int lod;
		System.Action updateCallback;

		public LODMesh(int lod, System.Action updateCallback) {
			this.lod = lod;
			this.updateCallback = updateCallback;
		}

		void OnMeshDataReceived(MeshData meshData) {
			// sets mesh object 
			mesh = meshData.CreateMesh ();
			hasMesh = true;

			updateCallback();
		}

		// tells the class it needs to request its mesh
 		public void RequestMesh(MapData mapData) {
			hasRequestedMesh = true;
			mapGenerator.RequestMeshData (mapData, lod, OnMeshDataReceived);
		}
	}

	[System.Serializable]
	public struct LODInfo {
		public int lod;
		public float visibleDstThreshold; //once user is outside this threshold, it will switch over to the next LOD (lower res version)
	}
}