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

		AddToWorld(grid, this, Vector3.up + Vector3.left); AddToWorld(grid, this, Vector3.up); AddToWorld(grid, this, Vector3.up + Vector3.right);
		AddToWorld(grid, this, Vector3.left); AddToWorld(grid, this); AddToWorld(grid, this, Vector3.right);
		AddToWorld(grid, this, Vector3.down + Vector3.left); AddToWorld(grid, this, Vector3.down); AddToWorld(grid, this, Vector3.down + Vector3.right);

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
