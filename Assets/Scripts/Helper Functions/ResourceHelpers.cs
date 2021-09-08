using UnityEngine;
using static GridHelpers;

public class ResourceHelpers : MonoBehaviour
{
	// Returns the resource of type T at the provided location, or null if there isn't a resource of type T there
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

	// Adds resource to the worldGrid
	public static void AddToWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().resourceObjects[GetPosInChunk(resource.transform.position).x, GetPosInChunk(resource.transform.position).y] = resource;
	}

	// Removes resource from the worldGrid
	public static void RemoveResourceFromWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().resourceObjects[GetPosInChunk(resource.transform.position).x, GetPosInChunk(resource.transform.position).y] = null;
	}
}
