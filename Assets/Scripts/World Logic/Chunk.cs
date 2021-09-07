using UnityEngine;

public class Chunk : MonoBehaviour
{
	public bool chunkActive = true;
	public Placeable[,] placeObjects = new Placeable[ChunkManager.CHUNK_SIZE, ChunkManager.CHUNK_SIZE];
	public BaseResource[,] resourceObjects = new BaseResource[ChunkManager.CHUNK_SIZE, ChunkManager.CHUNK_SIZE];
}
