using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	GameObject[,] placeObjects = new GameObject[ChunkManager.CHUNK_SIZE, ChunkManager.CHUNK_SIZE];
	GameObject[,] oreObjects = new GameObject[ChunkManager.CHUNK_SIZE, ChunkManager.CHUNK_SIZE];
}
