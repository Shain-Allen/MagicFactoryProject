using System;
using UnityEngine;
using static PlaceableHelpers;

public class SplitMergeRightLogic : ItemControl
{
	public SplitMergeLeftLogic leftPair;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;

		AddToWorld(grid, this);
		TryAttachFrontBelt();
		TryAttachBackBelt();
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this, 180);
	}

	public override void MoveItem()
	{
		leftPair.MoveItem();
	}

	public override void RemovedAction()
	{
		if (leftPair)
			leftPair.RemovedAction();

		base.RemovedAction();
	}
}
