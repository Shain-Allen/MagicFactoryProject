using System;
using UnityEngine;


public class HelpFuncs : MonoBehaviour
{
	// Calls Math.Round, but turns it into a float 
	private static float Round(float input, int degree)
	{
		return (float)Math.Round(input, degree);
	}

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
		float x = Round(Mathf.Sin(eulerAngle * Mathf.PI / 180), 3);
		float y = Round(Mathf.Cos(eulerAngle * Mathf.PI / 180), 3);
		return new Vector3(x, y, 0);
	}
	// Takes any Vector3 and turns the X/Y into an Euler Angle
	public static float VectorToEuler(Vector3 input)
	{
		float toReturn = (-Mathf.Atan(input.y / input.x) * 180 / Mathf.PI) + 90;
		if (input.x > 0)
			toReturn += 180;
		toReturn = Mathf.Abs(toReturn);
		return Round(toReturn, 1);
	}

	// Pythagorean Theorems two Vector2s
	public static float GetDistance(Vector2 a, Vector2 b)
	{
		return GetDistance(a.x, b.x, a.y, b.y);
	}
	public static float GetDistance(float x1, float x2, float y1, float y2)
	{
		return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
	}

	// Returns the chunk as a Vector2Int
	public static Vector2Int GetChunk(Vector2 input)
	{
		return GetChunk(input.x, input.y);
	}
	public static Vector2Int GetChunk(float x, float y)
	{
		int newX = (int)(x / ChunkManager.CHUNK_SIZE);
		int newY = (int)(y / ChunkManager.CHUNK_SIZE);
		newX = (x < 0) ? newX - 1 : newX;
		newY = (y < 0) ? newY - 1 : newY;
		return new Vector2Int(newX, newY);
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

	// Returns a Vector2Int of the chunk of the position in question
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

	// Returns a Vector2Int of the pos relative to the bottom left of the chunk
	public static Vector2Int PosToPosInChunk(float x, float y)
	{
		return PosToPosInChunk(new Vector2(x, y));
	}
	public static Vector2Int PosToPosInChunk(Vector2 pos)
	{
		Vector2Int chunkPos = ChunkToPos(GetChunk(pos));
		int x = (int)(pos.x - chunkPos.x);
		int y = (int)(pos.y - chunkPos.y);
		return new Vector2Int(x, y);
	}
}
