using UnityEngine;
using static GridHelpers;

public class PlaceableHelpers
{
	/*public static int R(float n)
	{
		return (int)(n + .5);
	}
	public static Vector2 R(Vector2 v)
	{
		v.x = (int)(v.x + .5);
		v.y = (int)(v.y + .5);
		return v;
	}
	public static Vector3 R(Vector3 v)
	{
		v.x = (int)(v.x + .5);
		v.y = (int)(v.y + .5);
		v.z = (int)(v.z + .5);
		return v;
	}*/
	// Returns the Placeable of type T at the provided location, or null if there isn't anything there
	public static T GetPlaceableAt<T>(GridControl grid, Vector2 pos) where T : Placeable
	{
		GameObject objAtPos, chunkParent;
		T placeableAtPos;

		if (grid.worldChunks.TryGetValue(GetChunk((pos)), out chunkParent))
		{
			objAtPos = chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(pos).x, PosToPosInChunk(pos).y];
			if (objAtPos != null && (objAtPos.TryGetComponent<T>(out placeableAtPos)))
				return placeableAtPos;
		}
		return default(T);
	}

	// Places the given Placeable into the world and the chunk array. PosOffSet alters the position of the placeable, used for large placeables
	public static void AddToWorld(GridControl grid, Placeable placeable)
	{
		AddToWorld(grid, placeable, Vector3.zero);
	}
	public static void AddToWorld(GridControl grid, Placeable placeable, Vector3 posOffSet)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, placeable.transform.position + posOffSet);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(placeable.transform.position + posOffSet).x, PosToPosInChunk(placeable.transform.position + posOffSet).y] = placeable.gameObject;
		Debug.Log($"Adding to world at ({PosToPosInChunk(placeable.transform.position + posOffSet).x}, {PosToPosInChunk(placeable.transform.position + posOffSet).y})");
	}

	// Remove the Placeable from any references to it. PosOffSet alters the position of the placeable, used for large placeables
	public static void RemoveFromWorld(GridControl grid, Placeable placeable)
	{
		RemoveFromWorld(grid, placeable, Vector3.zero);
	}
	public static void RemoveFromWorld(GridControl grid, Placeable placeable, Vector3 posOffSet)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, placeable.transform.position + posOffSet);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(placeable.transform.position + posOffSet).x, PosToPosInChunk(placeable.transform.position + posOffSet).y] = null;
	}
}