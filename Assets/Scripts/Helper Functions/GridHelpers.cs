using UnityEngine;

public class GridHelpers : MonoBehaviour
{
	// Returns the chunkParent object given any chunk's position (byChunk), or a standard position (byPos)
	public static GameObject GetChunkParentByPos(GridControl grid, Vector2 pos)
	{
		return GetChunkParentByChunk(grid, GetChunkPos(pos));
	}
	public static GameObject GetChunkParentByChunk(GridControl grid, Vector2Int chunkPos)
	{
		GameObject tempChunkParent;
		if (!grid.worldChunks.TryGetValue(chunkPos, out tempChunkParent))
			return null;
		return tempChunkParent;
	}

	// Returns the chunk's coordinates at the provided position as a Vector2Int
	public static Vector2Int GetChunkPos(Vector2 input)
	{
		int x = (int)Mathf.Round(input.x);
		int y = (int)Mathf.Round(input.y);
		// Due to rounding issues, negative values of x and y need to be adjusted
		x = (x < 0) ? x - 31 : x;
		y = (y < 0) ? y - 31 : y;
		x /= ChunkManager.CHUNK_SIZE;
		y /= ChunkManager.CHUNK_SIZE;
		return new Vector2Int(x, y);
	}

	// Returns the bottom-left (smallest X/Y) position coordinate in the given chunk 
	public static Vector2Int GetMinPosInChunk(Vector2Int chunkPos)
	{
		return new Vector2Int(chunkPos.x * ChunkManager.CHUNK_SIZE, chunkPos.y * ChunkManager.CHUNK_SIZE);
	}

	// Returns the position relative to the bottom left of the chunk
	public static Vector2Int GetPosInChunk(Vector2 pos)
	{
		Vector2Int toReturn = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		toReturn -= GetMinPosInChunk(GetChunkPos(pos));
		return toReturn;
	}

	/* GetChunkID converts the chunk X and Y into a single Int
	 * PRECONDITIONS: chunkX and chunkY should both be between roughly -30,000 and 30,000 to prevent OverFlows
	 * POSTCONDITIONS: Returned int will be between 0 and INT32MAX
	 * Recommend seeing SpiralChunkIDs.xlsx to better understand chunk IDs
	 */
	public static int GetChunkID(Vector2Int chunkPos)
	{
		int x = chunkPos.x;
		int y = chunkPos.y;

		int sprialLayer = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
		int topLeft = (int)Mathf.Pow(sprialLayer * 2 + 1, 2) - 1;
		int diff = Mathf.Abs(x + y);

		if (x < 0 && -x >= Mathf.Abs(y))
			return topLeft - diff;

		int bottomRight = (int)Mathf.Pow(sprialLayer * 2, 2);
		if (y < 0 && Mathf.Abs(y) >= Mathf.Abs(x))
			return bottomRight + diff;
		if (x > y)
			return bottomRight - diff;

		int topRight = bottomRight - (topLeft - bottomRight) / 2;
		diff = Mathf.Abs(x - y);
		return topRight - diff;
	}

	// Returns true if the given position is inside of the given boundaries
	public static bool insideBorder(Vector2 pos, Vector2 bottomLeft, Vector2 topRight)
	{
		return !(pos.x < bottomLeft.x || pos.y < bottomLeft.y || pos.x > topRight.x || pos.y > topRight.y);
	}
}
