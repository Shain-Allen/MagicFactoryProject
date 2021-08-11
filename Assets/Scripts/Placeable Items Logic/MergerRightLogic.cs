using System;
using UnityEngine;
using static PlaceableHelpers;

public class MergerRightLogic : ItemControl
{
	public MergerLeftLogic leftPair;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowFrontBelt = false;

		AddToWorld(grid, this);
		TryAttachBackBelt();
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this, 180);
	}

	public override void MoveItem()
	{
		// This should never be called
	}

	public override void RemovedAction()
	{
		if (leftPair)
			leftPair.RemovedAction();

		base.RemovedAction();
	}
}
