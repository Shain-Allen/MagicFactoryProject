using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

public class MergerLogic : ItemControl
{
	// inputICs[0] is the left-side input, inputICs[1] is the right-side input
	bool sideToMoveFromLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		if (GetPlaceableAt<Placeable>(grid_, transform.position + transform.right) != null)
		{
			RemovedAction();
			return;
		}

		inputICs = new ItemControl[2];
		relativePositions.Add(transform.right);
		inputValidRelPoses.Add(transform.right - transform.up);
		base.PlacedAction(grid_);
	}

	public override void setInput(ItemControl newIC)
	{
		if (newIC == null)
		{
			if (inputICs[0] && !GetPlaceableAt<ItemControl>(grid, inputValidRelPoses[0]))
				inputICs[0] = null;
			else if (inputICs[1] && !GetPlaceableAt<ItemControl>(grid, inputValidRelPoses[1]))
				inputICs[1] = null;
			else
				TryAttachInputs();
			return;
		}
		if (newIC.transform.position == transform.position - transform.up)
			inputICs[0] = newIC;
		else if (newIC.transform.position == transform.position - transform.up + transform.right)
			inputICs[1] = newIC;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		base.MoveItem(pullingIC);

		// Chain reaction backwards, prioritizing the side who's turn it is
		if (sideToMoveFromLeft)
		{
			sideToMoveFromLeft = false;
			if (inputICs[0])
				inputICs[0].MoveItem(this);
			if (inputICs[1])
				inputICs[1].MoveItem(this);
		}
		else
		{
			sideToMoveFromLeft = true;
			if (inputICs[1])
				inputICs[1].MoveItem(this);
			if (inputICs[0])
				inputICs[0].MoveItem(this);
		}
	}
}
