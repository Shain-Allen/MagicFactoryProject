using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticDrillLogic : Placeable
{
	private GridControl grid;

	public override void PlacedAction(GridControl _grid)
	{
		grid = _grid;
	}

	public override void RemovedAction()
	{

	}
}
