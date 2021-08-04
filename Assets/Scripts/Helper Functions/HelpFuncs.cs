using System;
using UnityEngine;

public class HelpFuncs : MonoBehaviour
{
	// Simple Function to ensure X and Y are both within the given bounds
	public static bool insideBorder(Vector2 pos, Vector2 bottomLeft, Vector2 topRight)
	{
		return insideBorder(pos.x, pos.y, bottomLeft.x, topRight.x, bottomLeft.y, topRight.y);
	}
	public static bool insideBorder(float x, float y, float left, float right, float bottom, float top)
	{
		return !(x < left || y < bottom || x > right || y > top);
	}

	// Takes an angle in Degrees and converts it to a 2D Vector3 of length 1, like a unit circle
	public static Vector3 EulerToVector(float eulerAngle)
	{
		eulerAngle *= -1;
		float x = (float)Math.Round(Math.Sin(eulerAngle * Math.PI / 180), 3);
		float y = (float)Math.Round(Math.Cos(eulerAngle * Math.PI / 180), 3);
		return new Vector3(x, y, 0);
	}
	// Takes any Vector3 and turns the X/Y into an Euler Angle
	public static float VectorToEuler(Vector3 input)
	{
		float toReturn = (float)(Math.Atan(input.y / input.x) * 180 / Math.PI) + 90;
		if (input.x > 0)
			toReturn += 180;
		return (float)Math.Round(toReturn, 1);
	}

	// Pythagorean Theorems two Vector2s
	public static float GetDistance(Vector2 a, Vector2 b)
	{
		return GetDistance(a.x, b.x, a.y, b.y);
	}
	public static float GetDistance(float x1, float x2, float y1, float y2)
	{
		return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
	}

	// Returns the chunk as a Vector2Int
	public static Vector2Int GetChunk(Vector2 input)
	{
		return GetChunk(input.x, input.y);
	}
	public static Vector2Int GetChunk(float x, float y)
	{
		return new Vector2Int((int)(x / ChunkManager.chunkSize), (int)(y / ChunkManager.chunkSize));
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
		int sprialLayer = Math.Max(Math.Abs(x), Math.Abs(y));
		int topLeft = (int)Math.Pow(sprialLayer * 2 + 1, 2) - 1;
		int diff = Math.Abs(x + y);

		if (x < 0 && -x >= Math.Abs(y))
			return topLeft - diff;

		int bottomRight = (int)Math.Pow(sprialLayer * 2, 2);
		if (y < 0 && Math.Abs(y) >= Math.Abs(x))
			return bottomRight + diff;
		if (x > y)
			return bottomRight - diff;

		int topRight = bottomRight - (topLeft - bottomRight) / 2;
		diff = Math.Abs(x - y);
		return topRight - diff;
	}

	// Returns a Vector2Int of the the bottom left position (smallest) in the chunk
	public static Vector2Int PosToChunk(float x, float y)
	{
		return PosToChunk(new Vector2(x, y));
	}
	public static Vector2Int PosToChunk(Vector2 pos)
	{
		int x = (int)(pos.x % ChunkManager.chunkSize);
		int y = (int)(pos.y % ChunkManager.chunkSize);
		return new Vector2Int(x, y);
	}

	// Returns a Vector2Int of the chunk of the position in question
	public static Vector2Int ChunkToPos(float x, float y)
	{
		return ChunkToPos(new Vector2(x, y));
	}
	public static Vector2Int ChunkToPos(Vector2 chunk)
	{
		int x = (int)(chunk.x * ChunkManager.chunkSize);
		int y = (int)(chunk.y * ChunkManager.chunkSize);
		return new Vector2Int(x, y);
	}

	// Returns a Vector2Int of the pos relative to the bottom left of the chunk
	public static Vector2Int PosToPosInChunk(float x, float y)
	{
		return PosToPosInChunk(new Vector2(x, y));
	}
	public static Vector2Int PosToPosInChunk(Vector2 pos)
	{
		Vector2Int chunkPos = PosToChunk(pos);
		int x = (int)(pos.x - chunkPos.x);
		int y = (int)(pos.y - chunkPos.y);
		return new Vector2Int(x, y);
	}
}
