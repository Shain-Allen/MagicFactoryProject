using UnityEngine;
using static GridHelpers;

public class ResourceHelpers : MonoBehaviour
{
	// Returns the Ore or Gas at the provided location, or null if there isn't an anything there
	public static T GetResourceAt<T>(GridControl grid, Vector2 pos) where T : BaseResource
	{
		GameObject objAtPos, chunkParent;
		T oreAtPos;

		if (grid.worldChunks.TryGetValue(GetChunk(pos), out chunkParent))
		{
			objAtPos = chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(pos).x, PosToPosInChunk(pos).y];
			if (objAtPos != null && (objAtPos.TryGetComponent<T>(out oreAtPos)))
			{
				return oreAtPos;
			}
		}
		return default(T);
	}

	// Places the given Resource into the world and the chunk array
	public static void AddToWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(resource.transform.position).x, PosToPosInChunk(resource.transform.position).y] = resource.gameObject;

	}

	// Removes the Resource given from the world grid
	public static void RemoveResourceFromWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(resource.transform.position).x, PosToPosInChunk(resource.transform.position).y] = null;
	}
}
