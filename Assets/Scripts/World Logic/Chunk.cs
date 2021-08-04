using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	GameObject[,] placeObjects = new GameObject[ChunkManager.chunkSize, ChunkManager.chunkSize];
	GameObject[,] oreObjects = new GameObject[ChunkManager.chunkSize, ChunkManager.chunkSize];
}
