using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

public class MergerLogic : ItemControl
{
	ItemControl leftInput, rightInput;
	bool sideToMoveFromLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		if (GetPlaceableAt<Placeable>(grid_, transform.position + transform.right) != null)
			return;

		relativePositions.Add(transform.right);
		inputValidRelPoses.Add(transform.right - transform.up);
		base.PlacedAction(grid_);
	}

	public override void setInput(ItemControl newIC)
	{
		if (newIC.transform.position == transform.position - transform.up)
			leftInput = newIC;
		else if (newIC.transform.position == transform.position - transform.up + transform.right)
			rightInput = newIC;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		base.MoveItem(pullingIC);

		// Chain reaction backwards, prioritizing the side who's turn it is
		if (sideToMoveFromLeft)
		{
			sideToMoveFromLeft = false;
			if (leftInput)
				leftInput.MoveItem(this);
			if (rightInput)
				rightInput.MoveItem(this);
		}
		else
		{
			sideToMoveFromLeft = true;
			if (rightInput)
				rightInput.MoveItem(this);
			if (leftInput)
				leftInput.MoveItem(this);
		}
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, transform.right);
		RemoveFromWorld(grid, this);

		if (leftInput)
			leftInput.TryAttachOutputs();
		leftInput = null;

		if (rightInput)
			rightInput.TryAttachOutputs();
		rightInput = null;

		if (outputIC)
			outputIC.TryAttachInputs();
		outputIC = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}
}
