using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;

public abstract class Placeable : MonoBehaviour
{
	protected GridControl grid;
	protected List<Vector3> positions = new List<Vector3>();

	/* PlacedAction initializes the variables inside this Placeable and adds this to the world grid
	 * PRECONDITIONS: There shouldn't be another placeable at this one's location
	 * POSTCONDITIONS: This will be placed into the world grid at this one's location
	 */
	public virtual void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		positions.Add(transform.position);
		foreach (Vector3 pos in positions)
			AddToWorld(grid, this, pos - transform.position);
	}

	/* RemovedAction removes this from existance
	 * No special Preconditions
	 * POSTCONDITIONS: This will be removed from the world grid and the gameObject will be destroyed
	 * Inheriting classes should ensure all references to this object are also removed
	 */
	public virtual void RemovedAction()
	{
		foreach (Vector3 pos in positions)
			RemoveFromWorld(grid, this, pos - transform.position);
		Destroy(gameObject);
	}

	// Returns a list of all the positions this IC occupies, necessary for multi-tile structures
	public virtual List<Vector3> getAllPositions() { return positions; }
}
