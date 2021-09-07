using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridHelpers;

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
		return GetChunkPos(botLeft);
	}

	public static Vector2Int getTopRightBound(GameObject cam)
	{
		Camera camera = cam.GetComponent<Camera>();
		Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
		return GetChunkPos(topRight);
	}

	public static void LoadChunks(GridControl grid, Vector2Int bottomLeftBound, Vector2Int topRightBound)
	{
		GameObject isChunkLoaded;
		Vector2Int tempPos;

		for (int x = bottomLeftBound.x - BUFFER; x <= topRightBound.x + BUFFER; x++)
		{
			for (int y = bottomLeftBound.y - BUFFER; y <= topRightBound.y + BUFFER; y++)
			{
				tempPos = new Vector2Int(x, y);
				if (!grid.worldChunks.TryGetValue(tempPos, out isChunkLoaded) || isChunkLoaded == null)
					OreGeneration.LoadChunkResources(grid, grid.worldSeed, tempPos);
			}
		}
	}

	public static void UnloadChunks(GridControl grid, Vector2Int bottomLeftBound, Vector2Int topRightBound)
	{
		GameObject chunkToDelete;

		// Get all the Vector2Ints of the loaded chunks, but in a way it doesn't matter that I delete them
		bottomLeftBound = new Vector2Int(bottomLeftBound.x - BUFFER, bottomLeftBound.y - BUFFER);
		topRightBound = new Vector2Int(topRightBound.x + BUFFER, topRightBound.y + BUFFER);
		List<Vector2Int> loadedChunkPositions = new List<Vector2Int>();
		foreach (Vector2Int LoadedChunkPos in grid.worldChunks.Keys)
			loadedChunkPositions.Add(LoadedChunkPos);

		Vector2Int loadedChunkPos;
		for (int i = 0; i < loadedChunkPositions.Count; i++)
		{
			loadedChunkPos = loadedChunkPositions[i];
			if (grid.worldChunks.TryGetValue(loadedChunkPos, out chunkToDelete) && chunkToDelete != null)
			{
				// If the chunk is outside of the buffer, unload the chunk
				if (!insideBorder(loadedChunkPos, bottomLeftBound, topRightBound))
				{
					// Call RemovedAction on the border of the chunk so ICs will reset their connections
					Chunk tempChunk = chunkToDelete.GetComponent<Chunk>();
					for (int j = 0; j < ChunkManager.CHUNK_SIZE - 1; j++)
					{
						DeletePlaceableHelper(tempChunk, j, 0);
						DeletePlaceableHelper(tempChunk, j + 1, 31);
						DeletePlaceableHelper(tempChunk, 0, j + 1);
						DeletePlaceableHelper(tempChunk, 31, j);
					}

					grid.worldChunks.Remove(loadedChunkPos);
					Destroy(chunkToDelete);
				}
			}
		}
	}

	private static void DeletePlaceableHelper(Chunk tempChunk, int xToDel, int yToDel)
	{
		if (tempChunk.placeObjects[xToDel, yToDel] == null)
			return;
		Placeable tempPlaceable;
		if (tempChunk.placeObjects[xToDel, yToDel].TryGetComponent<Placeable>(out tempPlaceable))
			tempPlaceable.RemovedAction();
	}
}
