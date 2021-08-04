using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	GameObject[,] placeObjects = new GameObject[OreGeneration.chunkSize, OreGeneration.chunkSize];
	GameObject[,] oreObjects = new GameObject[OreGeneration.chunkSize, OreGeneration.chunkSize];
}
