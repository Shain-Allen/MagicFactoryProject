using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	// The size of chunks! Chunks are squares, so this is both the height and the width
	public static int chunkSize = 32;

	GameObject[,] placeObjects = new GameObject[chunkSize, chunkSize];
	GameObject[,] oreObjects = new GameObject[chunkSize, chunkSize];
}
