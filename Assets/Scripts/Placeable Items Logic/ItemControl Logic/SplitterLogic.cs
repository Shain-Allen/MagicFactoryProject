using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

public class SplitterLogic : ItemControl
{
	public ItemControl leftOutput, rightOutput;
	bool nextOutputLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		if (GetPlaceableAt<Placeable>(grid_, transform.position + transform.right) != null)
			return;

		positions.Add(transform.position + transform.right);
		base.PlacedAction(grid_);
	}

	public override void TryAttachOutputs()
	{
		TryAttachOutputHelper(grid, this);
		ItemControl frontRightSideIC = GetPlaceableAt<ItemControl>(grid, transform.position + transform.up + transform.right);
		if (frontRightSideIC)
			TryAttachOutputHelper(grid, this, frontRightSideIC);
	}
	public override void setOutput(ItemControl newIC)
	{
		if (newIC.transform.position == transform.position + transform.up)
			leftOutput = newIC;
		else if (newIC.transform.position == transform.position + transform.up + transform.right)
			rightOutput = newIC;
	}
	public override bool AllowOutputTo(ItemControl askingIC)
	{
		if (askingIC.transform.position == transform.position + transform.up)
			if (leftOutput == null)
				return true;
		if (askingIC.transform.position == transform.position + transform.up + transform.right)
			if (rightOutput == null)
				return true;
		return false;
	}
	public override void setOutputToNull(ItemControl deletingIC)
	{
		if (deletingIC.transform.position == transform.position + transform.up)
			leftOutput = null;
		else if (deletingIC.transform.position == transform.position + transform.up + transform.right)
			rightOutput = null;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		// Prevents MoveItem from being called twice each BeltCycle by only responding to calls from left side
		if (pullingIC != leftOutput && pullingIC != null)
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
		if (inputIC)
			inputIC.MoveItem(this);
	}
	private ItemControl chooseOutputIC()
	{
		bool leftOutputFree = leftOutput && leftOutput.AllowItem(this);
		bool rightOutputFree = rightOutput && rightOutput.AllowItem(this);

		// If it's left's turn and left is free, choose it
		if (nextOutputLeft && leftOutputFree)
		{
			nextOutputLeft = false;
			return leftOutput;
		}
		// But if it's right's turn and right is free, choose it
		else if (!nextOutputLeft && rightOutputFree)
		{
			nextOutputLeft = true;
			return rightOutput;
		}
		// If it's right's turn but left is free, choose it
		else if (leftOutputFree)
		{
			nextOutputLeft = false;
			return leftOutput;
		}
		// If it's left's turn but right is free, choose it
		else if (rightOutputFree)
		{
			nextOutputLeft = true;
			return rightOutput;
		}
		// If neither are free, will return null
		return null;
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, transform.right);
		RemoveFromWorld(grid, this);

		if (inputIC)
		{
			inputIC.setOutputToNull(this);
			inputIC.TryAttachOutputs();
		}
		inputIC = null;

		if (leftOutput)
		{
			leftOutput.setInputToNull(this);
			leftOutput.TryAttachInputs();
		}
		leftOutput = null;

		if (rightOutput)
		{
			rightOutput.setInputToNull(this);
			rightOutput.TryAttachInputs();
		}
		rightOutput = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}
}
