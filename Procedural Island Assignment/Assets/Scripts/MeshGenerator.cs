using UnityEngine;
using System.Collections;

// this class is used to generate all of the necessary data needed for a mesh
public static class MeshGenerator {

	// function that creates mesh data which can be applied to a mesh in the MeshData class
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail) {
		AnimationCurve heightCurve = new AnimationCurve (_heightCurve.keys);

		// gets width and height of the mesh data that is applied to the mesh in the Mesh Data Class
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		// these variables allow for the mesh to be centered
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
		int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

		// the local meshData variable
		MeshData meshData = new MeshData (verticesPerLine, verticesPerLine);

		// keeps track of current vertex index
		int vertexIndex = 0;

		for (int y = 0; y < height; y += meshSimplificationIncrement) {
			for (int x = 0; x < width; x += meshSimplificationIncrement) {
				// finds vertex position based on the x and z position, then sets the y based on the curve and multiplier
				meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate (heightMap [x, y]) * heightMultiplier, topLeftZ - y);
				// sets all the UVs for the texture
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					// creates the two triangles for the two squares
					meshData.AddTriangle (vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle (vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
				}

				// increments vertexIndex
				vertexIndex++;
			}
		}

		return meshData;

	}
}

// class which contains all the information to create a meshdata object
public class MeshData {
	// mesh vertices
	public Vector3[] vertices;
	// mesh triangles
	public int[] triangles;
	// mesh uvs
	public Vector2[] uvs;
	// variable used to keep track of which triangle is being worked on
	int triangleIndex;

	// constructor of meshdata object which sets all of its start values based on the given height and width
	public MeshData(int meshWidth, int meshHeight) {
		// finds out the amount of vertices needed
		vertices = new Vector3[meshWidth * meshHeight];
		// finds out the amount of uvs needed
		uvs = new Vector2[meshWidth * meshHeight];
		// finds out the amount of triangles needed
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	// function that creates a triangle based on the vertex index numbers
	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	// function that applies mesh data information to a mesh and returns it
	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}

}