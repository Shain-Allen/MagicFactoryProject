using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;

public class ChunkManager : MonoBehaviour
{
	// The size of chunks! Chunks are squares, so this is both the height and the width
	public const int CHUNK_SIZE = 32;
	//BUFFER is the extra # of chunks loaded beyond the Camera's view
	private const int BUFFER = 1;

	public static Vector2Int getBottomLeftBound(GameObject cam)
	{
		Camera camera = cam.GetComponent<Camera>();
		Vector3 botLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
		return GetChunk(botLeft.x, botLeft.y);
	}

	public static Vector2Int getTopRightBound(GameObject cam)
	{
		Camera camera = cam.GetComponent<Camera>();
		Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
		return GetChunk(topRight.x, topRight.y);
	}

	public static void LoadChunks(GridControl grid, Vector2Int bottomLeftBound, Vector2Int topRightBound)
	{
		GameObject isChunkLoaded;

		for (int x = bottomLeftBound.x - BUFFER; x <= topRightBound.x + BUFFER; x++)
		{
			for (int y = bottomLeftBound.y - BUFFER; y <= topRightBound.y + BUFFER; y++)
			{
				if (!grid.worldChunks.TryGetValue(new Vector2Int(x, y), out isChunkLoaded) || isChunkLoaded == null)
					OreGeneration.LoadChunkOres(grid, grid.worldSeed, x, y);
			}
		}
	}

	public static void UnloadChunks(GridControl grid, Vector2Int bottomLeftBound, Vector2Int topRightBound)
	{
		GameObject isChunkLoaded;

		// Get all the Vector2Ints of the loaded chunks, but in a way it doesn't matter that I delete them
		bottomLeftBound = new Vector2Int(bottomLeftBound.x - BUFFER, bottomLeftBound.y - BUFFER);
		topRightBound = new Vector2Int(topRightBound.x + BUFFER, topRightBound.y + BUFFER);
		List<Vector2Int> loadedChunkPositions = new List<Vector2Int>();
		foreach (Vector2Int LoadedChunkPos in grid.worldChunks.Keys)
			loadedChunkPositions.Add(LoadedChunkPos);

		Vector2Int loadedChunkPos;
		GameObject tempOre;
		for (int i = 0; i < loadedChunkPositions.Count; i++)
		{
			loadedChunkPos = loadedChunkPositions[i];
			// If the chunk is outside of the buffer, unload the chunk
			if (grid.worldChunks.TryGetValue(loadedChunkPos, out isChunkLoaded) && isChunkLoaded != null)
			{
				if (!insideBorder(loadedChunkPos, bottomLeftBound, topRightBound))
				{
					for (int x = loadedChunkPos.x * CHUNK_SIZE; x < (loadedChunkPos.x + 1) * CHUNK_SIZE; x++)
						for (int y = loadedChunkPos.y * CHUNK_SIZE; y < (loadedChunkPos.y + 1) * CHUNK_SIZE; y++)
							if (grid.oreObjects.TryGetValue(loadedChunkPos, out tempOre))
								grid.oreObjects.Remove(loadedChunkPos);

					grid.worldChunks.Remove(loadedChunkPos);
					Destroy(isChunkLoaded);
				}
			}
		}
	}
}
