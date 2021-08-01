using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
	public abstract void PlacedAction(GridControl grid);

	public abstract void RemovedAction();
}
