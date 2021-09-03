using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

public class SplitterLogic : ItemControl
{
	// outputICs[0] is the left-side output, outputICs[1] is the right-side output
	bool nextOutputLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		if (GetPlaceableAt<Placeable>(grid_, transform.position + transform.right) != null)
		{
			RemovedAction();
			return;
		}

		outputICs = new ItemControl[2];
		relativePositions.Add(transform.right);
		outputValidRelPoses.Add(transform.right + transform.up);
		base.PlacedAction(grid_);
	}

	public override void setOutput(ItemControl newIC)
	{
		if (newIC == null)
		{
			if (outputICs[0] && !GetPlaceableAt<ItemControl>(grid, outputValidRelPoses[0]))
				outputICs[0] = null;
			else if (outputICs[1] && !GetPlaceableAt<ItemControl>(grid, outputValidRelPoses[1]))
				outputICs[1] = null;
			else
				TryAttachOutputs();
			return;
		}
		if (newIC.transform.position == transform.position + transform.up)
			outputICs[0] = newIC;
		else if (newIC.transform.position == transform.position + transform.up + transform.right)
			outputICs[1] = newIC;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		// Prevents MoveItem from being called twice each BeltCycle by only responding to calls from left side
		if (pullingIC != outputICs[0] && pullingIC != null)
			return;

		ItemControl outputBelt = chooseOutputIC();
		// Try to move the item
		if (itemSlot && outputBelt)
		{
			StartCoroutine(SmoothMove(grid, itemSlot, itemSlot.transform.position, outputBelt.transform.position));
			outputBelt.setItemSlot(this, itemSlot);
			itemSlot = null;
		}

		// Chain reaction backwards
		if (inputICs[0])
			inputICs[0].MoveItem(this);
	}

	private ItemControl chooseOutputIC()
	{
		bool leftOutputFree = outputICs[0] && outputICs[0].AllowItem(this);
		bool rightOutputFree = outputICs[1] && outputICs[1].AllowItem(this);

		// If it's left's turn and left is free, choose it
		if (nextOutputLeft && leftOutputFree)
		{
			nextOutputLeft = false;
			return outputICs[0];
		}
		// But if it's right's turn and right is free, choose it
		else if (!nextOutputLeft && rightOutputFree)
		{
			nextOutputLeft = true;
			return outputICs[1];
		}
		// If it's right's turn but left is free, choose it
		else if (leftOutputFree)
		{
			nextOutputLeft = false;
			return outputICs[0];
		}
		// If it's left's turn but right is free, choose it
		else if (rightOutputFree)
		{
			nextOutputLeft = true;
			return outputICs[1];
		}
		// If neither are free, will return null
		return null;
	}
}
