using System.Collections.Generic;
using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
	public abstract void PlacedAction(GridControl grid_);

	public abstract void RemovedAction();

	// Returns a list of all the positions this IC occupies, necessary for multi-tile structures
	public virtual List<Vector3> getAllPositions()
	{
		List<Vector3> toReturn = new List<Vector3>();
		toReturn.Add(transform.position);
		return toReturn;
	}
}
