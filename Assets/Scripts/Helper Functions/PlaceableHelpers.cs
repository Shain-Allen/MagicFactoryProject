using UnityEngine;
using static GridHelpers;

public class PlaceableHelpers
{
	// Returns the Placeable of type T at the provided location, or null if there isn't a placeable of type T there
	public static T GetPlaceableAt<T>(GridControl grid, Vector2 pos) where T : Placeable
	{
		GameObject chunkParent;
		Placeable placeableAtPos;
		T typeAtPos;

		if (grid.worldChunks.TryGetValue(GetChunkPos((pos)), out chunkParent))
		{
			placeableAtPos = chunkParent.GetComponent<Chunk>().placeObjects[GetPosInChunk(pos).x, GetPosInChunk(pos).y];
			if (placeableAtPos != null && (placeableAtPos.TryGetComponent<T>(out typeAtPos)))
				return typeAtPos;
		}
		return default(T);
	}

	// Adds placeable into the worldGrid. PosOffSet alters the position of the placeable, used for multi-tile placeables
	public static void AddToWorld(GridControl grid, Placeable placeable, Vector3 posOffSet)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, placeable.transform.position + posOffSet);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[GetPosInChunk(placeable.transform.position + posOffSet).x, GetPosInChunk(placeable.transform.position + posOffSet).y] = placeable;
	}

	// Remove placeable from the worldGrid. PosOffSet alters the position of the placeable, used for multi-tile placeables
	public static void RemoveFromWorld(GridControl grid, Placeable placeable, Vector3 posOffSet)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, placeable.transform.position + posOffSet);
		if (chunkParent)
		{
			Vector2Int pos = GetPosInChunk(placeable.transform.position + posOffSet);
			if (chunkParent.GetComponent<Chunk>().placeObjects[pos.x, pos.y] == placeable)
				chunkParent.GetComponent<Chunk>().placeObjects[pos.x, pos.y] = null;
		}
	}
}