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
		TryAttachBackBeltHelper(grid, this);
	}

	public override void MoveItem(ItemControl pullingIC)
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
