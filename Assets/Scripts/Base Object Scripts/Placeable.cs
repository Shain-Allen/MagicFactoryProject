using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
	public abstract void PlacedAction(GridControl grid_);

	public abstract void RemovedAction();
}
