using UnityEngine;
using static GridHelpers;

public class ResourceHelpers : MonoBehaviour
{
	// Returns the Ore or Gas at the provided location, or null if there isn't an anything there
	public static T GetResourceAt<T>(GridControl grid, Vector2 pos) where T : BaseResource
	{
		GameObject chunkParent;
		BaseResource resourceAtPos;
		T typeAtPos;

		if (grid.worldChunks.TryGetValue(GetChunkPos(pos), out chunkParent))
		{
			resourceAtPos = chunkParent.GetComponent<Chunk>().resourceObjects[GetPosInChunk(pos).x, GetPosInChunk(pos).y];
			if (resourceAtPos != null && (resourceAtPos.TryGetComponent<T>(out typeAtPos)))
			{
				return typeAtPos;
			}
		}
		return default(T);
	}

	// Places the given resource into the world grid
	public static void AddToWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().resourceObjects[GetPosInChunk(resource.transform.position).x, GetPosInChunk(resource.transform.position).y] = resource;
	}

	// Removes the given resource from the world grid
	public static void RemoveResourceFromWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().resourceObjects[GetPosInChunk(resource.transform.position).x, GetPosInChunk(resource.transform.position).y] = null;
	}
}
