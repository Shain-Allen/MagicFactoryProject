using System;
using System.Collections.Generic;
using UnityEngine;
using static MathHelpers;
using static PlaceableHelpers;
using static ICHelpers;

public class SplitterLogic : ItemControl
{
	public ItemControl leftOutput = null;
	public ItemControl rightOutput = null;
	Vector3 rightOffset, frontOffset;
	bool nextOutputLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		rightOffset = EulerToVector(transform.rotation.eulerAngles.z + 270);
		frontOffset = EulerToVector(transform.rotation.eulerAngles.z);
		if (GetPlaceableAt<Placeable>(grid, transform.position + rightOffset) != null)
			return;

		grid.OnBeltTimerCycle += BeltCycle;
		AddToWorld(grid, this);
		AddToWorld(grid, this, rightOffset);
		TryAttachFrontBelt();
		TryAttachBackBelt();
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
		ItemControl frontRightSideIC = GetPlaceableAt<ItemControl>(grid, transform.position + frontOffset + rightOffset);
		if (frontRightSideIC)
			TryAttachFrontBeltHelper(grid, this, frontRightSideIC);
	}

	public override void setFrontBelt(ItemControl newIC)
	{
		if (newIC.transform.position == transform.position + frontOffset)
			leftOutput = newIC;
		else if (newIC.transform.position == transform.position + frontOffset + rightOffset)
			rightOutput = newIC;
	}

	public override List<Vector3> getAllPositions()
	{
		List<Vector3> toReturn = new List<Vector3>();
		toReturn.Add(transform.position);
		toReturn.Add(transform.position + rightOffset);
		return toReturn;
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this);
	}

	public override bool AllowFrontBeltTo(ItemControl askingIC)
	{
		if (askingIC.transform.position == transform.position + frontOffset)
			if (leftOutput == null)
				return true;
		if (askingIC.transform.position == transform.position + frontOffset + rightOffset)
			if (rightOutput == null)
				return true;
		return false;
	}

	public override void setFrontBeltToNull(ItemControl deletingIC)
	{
		if (deletingIC.transform.position == transform.position + frontOffset)
			leftOutput = null;
		else if (deletingIC.transform.position == transform.position + frontOffset + rightOffset)
			rightOutput = null;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		// Prevents MoveItem from being called twice each BeltCycle by only responding to calls from left side
		if (pullingIC != leftOutput && pullingIC != null)
			return;

		ItemControl outputBelt = chooseOutputBelt();
		// Try to move the item
		if (itemSlot && outputBelt)
		{
			StartCoroutine(SmoothMove(grid, itemSlot, itemSlot.transform.position, outputBelt.transform.position));
			outputBelt.setItemSlot(this, itemSlot);
			itemSlot = null;
		}

		// Chain reaction backwards
		if (backBelt)
			backBelt.MoveItem(this);
	}

	private ItemControl chooseOutputBelt()
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

	// If this belt is in front, start a chain reaction backwards for movement
	public void BeltCycle(object sender, EventArgs e)
	{
		if (leftOutput == null)
			MoveItem(null);
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, rightOffset);
		RemoveFromWorld(grid, this);

		if (backBelt)
		{
			backBelt.setFrontBeltToNull(this);
			backBelt.TryAttachFrontBelt();
		}
		backBelt = null;

		if (leftOutput)
		{
			leftOutput.setBackBeltToNull(this);
			leftOutput.TryAttachBackBelt();
		}
		leftOutput = null;

		if (rightOutput)
		{
			rightOutput.setBackBeltToNull(this);
			rightOutput.TryAttachBackBelt();
		}
		rightOutput = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}
}
