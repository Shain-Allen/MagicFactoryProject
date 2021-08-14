using System;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;

public class MysticDrillLogic : Placeable
{
	private GridControl grid;

	private Vector2 outputLocation;

	public override void PlacedAction(GridControl _grid)
	{
		grid = _grid;
		outputLocation = transform.up * 2;

		AddToWorld(grid, this);

		grid.Tick += TryMineOre;
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this);
		Destroy(gameObject);
	}

	public void TryMineOre(object sender, EventArgs e)
	{

	}
}
