using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public bool chunkActive = true;
	public GameObject[,] placeObjects = new GameObject[ChunkManager.CHUNK_SIZE, ChunkManager.CHUNK_SIZE];
	public GameObject[,] oreObjects = new GameObject[ChunkManager.CHUNK_SIZE, ChunkManager.CHUNK_SIZE];

	//test comment please delete
}
