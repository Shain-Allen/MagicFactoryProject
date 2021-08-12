using System;
using UnityEngine;
using static PlaceableHelpers;

public class SplitterRightLogic : ItemControl
{
	public SplitterLeftLogic leftPair;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowBackBelt = false;

		AddToWorld(grid, this);
		TryAttachFrontBelt();
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
	}

	public override void MoveItem()
	{
		// Don't do anything to prevent the backBelt from being pulled forward twice
	}

	public override void RemovedAction()
	{
		if (leftPair)
			leftPair.RemovedAction();

		base.RemovedAction();
	}
}
