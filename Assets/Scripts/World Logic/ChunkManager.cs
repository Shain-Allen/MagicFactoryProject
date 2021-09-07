using UnityEngine;
using static GridHelpers;

public class ChunkManager : MonoBehaviour
{
	// The size of chunks! Chunks are squares, so this is both the height and the width
	public const int CHUNK_SIZE = 32;
	//BUFFER is the extra # of chunks loaded beyond the Camera's view
	private const int BUFFER = 1;

	public static void LoadChunks(GridControl grid, GameObject cam)
	{
		Vector2Int bottomLeftBound = getBottomLeftBound(cam);
		Vector2Int topRightBound = getTopRightBound(cam);
		GameObject isChunkLoaded;
		Vector2Int tempPos;

		for (int x = bottomLeftBound.x - BUFFER; x <= topRightBound.x + BUFFER; x++)
		{
			for (int y = bottomLeftBound.y - BUFFER; y <= topRightBound.y + BUFFER; y++)
			{
				tempPos = new Vector2Int(x, y);
				// If the chunk isn't instantiated, generate it
				if (!grid.worldChunks.TryGetValue(tempPos, out isChunkLoaded) || isChunkLoaded == null)
					OreGeneration.GenerateChunkResources(grid, grid.worldSeed, tempPos);
				// If the chunk was instantiated but unloaded, reload it
				else
					setVisiblity(isChunkLoaded, true);
			}
		}
	}

	public static void UnloadChunks(GridControl grid, GameObject cam)
	{
		Vector2Int bottomLeftBound = getBottomLeftBound(cam);
		Vector2Int topRightBound = getTopRightBound(cam);
		GameObject chunkToUnload;

		// Get all the Vector2Ints of the loaded chunks, but in a way it doesn't matter that I delete them
		bottomLeftBound = new Vector2Int(bottomLeftBound.x - BUFFER, bottomLeftBound.y - BUFFER);
		topRightBound = new Vector2Int(topRightBound.x + BUFFER, topRightBound.y + BUFFER);
		foreach (Vector2Int loadedChunkPos in grid.worldChunks.Keys)
		{
			grid.worldChunks.TryGetValue(loadedChunkPos, out chunkToUnload);
			if (!insideBorder(loadedChunkPos, bottomLeftBound, topRightBound))
				setVisiblity(chunkToUnload, false);
		}
	}

	private static void setVisiblity(GameObject chunk, bool setting)
	{
		Chunk tempChunk = chunk.GetComponent<Chunk>();
		if (tempChunk.chunkActive == setting)
			return;
		foreach (GameObject placeable in tempChunk.placeObjects)
			if (placeable)
				placeable.GetComponent<SpriteRenderer>().enabled = setting;
		foreach (GameObject resource in tempChunk.oreObjects)
			if (resource)
				resource.GetComponent<SpriteRenderer>().enabled = setting;
		tempChunk.chunkActive = setting;
	}

	private static Vector2Int getBottomLeftBound(GameObject cam)
	{
		Camera camera = cam.GetComponent<Camera>();
		Vector3 botLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
		return GetChunkPos(botLeft);
	}
	private static Vector2Int getTopRightBound(GameObject cam)
	{
		Camera camera = cam.GetComponent<Camera>();
		Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
		return GetChunkPos(topRight);
	}

}
