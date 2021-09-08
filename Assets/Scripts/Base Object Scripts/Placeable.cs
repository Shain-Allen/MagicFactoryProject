using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;

/* Placeables are any and everything that can be placed into the world by the player
 * Placeables are stored in the worldGrid, and each position can only have 1 placeable at a time
 * They can be multi-position structures, but are by default only 1x1, centered around the transform.position
 */
public abstract class Placeable : MonoBehaviour
{
	protected GridControl grid;
	// relativePositions stores every position this occupies, relative to the center which is Vector3.zero
	protected List<Vector3> relativePositions = new List<Vector3>();

	/* PlacedAction completely initializes this Placeable and adds this to the world grid
	 * PRECONDITIONS: relativePositions has been modified if this Placeable is larger than 1x1
	 * POSTCONDITIONS: If any of the positions in this are occupied, PlacedAction will cancel
	 * cont.: Otherwise, this is added to the worldGrid for every relativePosition it occupies 
	 */
	public virtual void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		relativePositions.Add(Vector3.zero);
		foreach (Vector3 pos in relativePositions)
		{
			if (GetPlaceableAt<Placeable>(grid_, transform.position + pos) != null)
			{
				RemovedAction();
				return;
			}
		}
		foreach (Vector3 pos in relativePositions)
			AddToWorld(grid, this, pos);
	}

	/* RemovedAction removes this from the worldGrid and returns memory to the system
	 * No special Preconditions
	 * POSTCONDITIONS: This will be removed from the worldGrid and this will be destroyed
	 * Inheriting classes should ensure all references to this object are also removed
	 */
	public virtual void RemovedAction()
	{
		foreach (Vector3 pos in relativePositions)
			RemoveFromWorld(grid, this, pos);
		Destroy(gameObject);
	}

	// Returns a list of all the positions this Placeable occupies
	public virtual List<Vector3> getAllPositions()
	{
		List<Vector3> toReturn = new List<Vector3>();
		foreach (Vector3 pos in relativePositions)
			toReturn.Add(pos + transform.position);
		return toReturn;
	}
}
