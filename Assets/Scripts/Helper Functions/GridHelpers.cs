using UnityEngine;

public class GridHelpers : MonoBehaviour
{
	public static Vector2Int Round(Vector2 pos)
	{
		Vector2Int newPos = new Vector2Int();
		newPos.x = (int)Mathf.Round(pos.x);
		newPos.y = (int)Mathf.Round(pos.y);
		return newPos;
	}

	// Returns the parent chunk object given any chunk's position, or a standard position
	public static GameObject GetChunkParentByPos(GridControl grid, Vector2 pos)
	{
		return GetChunkParentByChunk(grid, GetChunk(pos));
	}
	public static GameObject GetChunkParentByChunk(GridControl grid, Vector2Int chunkPos)
	{
		GameObject tempChunkParent;
		if (!grid.worldChunks.TryGetValue(chunkPos, out tempChunkParent))
			return null;
		return tempChunkParent;
	}

	// Returns the chunk at the provided position as a Vector2Int
	public static Vector2Int GetChunk(Vector2 input)
	{
		input = Round(input);
		return GetChunk(input.x, input.y);
	}
	public static Vector2Int GetChunk(float x, float y)
	{
		// Due to rounding issues, negative values of x and y need to be adjusted
		x = (x < 0) ? x - 31 : x;
		y = (y < 0) ? y - 31 : y;
		int chunkX = ((int)x / ChunkManager.CHUNK_SIZE);
		int chunkY = ((int)y / ChunkManager.CHUNK_SIZE);
		return new Vector2Int(chunkX, chunkY);
	}

	/* GetChunkID converts the chunk X and Y into a single Int
	 * PRECONDITIONS: chunkX and chunkY should both be between roughly -30,000 and 30,000 to prevent OverFlows
	 * POSTCONDITIONS: Returned int will be bwtween 0 and INT32MAX
	 * Recommend seeing SpiralChunkIDs.xlsx to better understand chunk IDs
	 */
	public static int GetChunkID(Vector2Int pos)
	{
		return GetChunkID(pos.x, pos.y);
	}
	public static int GetChunkID(int x, int y)
	{
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

	// Returns the left-bottom-most position in the provided chunk
	public static Vector2Int ChunkToPos(float x, float y)
	{
		return ChunkToPos(new Vector2(x, y));
	}
	public static Vector2Int ChunkToPos(Vector2 chunk)
	{
		int x = (int)(chunk.x * ChunkManager.CHUNK_SIZE);
		int y = (int)(chunk.y * ChunkManager.CHUNK_SIZE);
		return new Vector2Int(x, y);
	}

	// Returns the position relative to the bottom left of the chunk
	public static Vector2Int PosToPosInChunk(float x, float y)
	{
		return PosToPosInChunk(new Vector2(x, y));
	}
	public static Vector2Int PosToPosInChunk(Vector2 pos)
	{
		pos = Round(pos);
		Vector2Int chunkPos = ChunkToPos(GetChunk(pos));
		int x = (int)(pos.x - chunkPos.x);
		int y = (int)(pos.y - chunkPos.y);
		return new Vector2Int(x, y);
	}
}
