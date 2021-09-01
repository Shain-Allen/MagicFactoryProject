using System;
using System.Collections.Generic;
using UnityEngine;
using static MathHelpers;
using static PlaceableHelpers;
using static ICHelpers;

public class MergerLogic : ItemControl
{
	ItemControl leftInput = null;
	ItemControl rightInput = null;
	Vector3 rightOffset, backOffset;
	bool sideToMoveFromLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		rightOffset = EulerToVector(transform.rotation.eulerAngles.z + 270);
		backOffset = EulerToVector(transform.rotation.eulerAngles.z + 180);
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
	}

	public override List<Vector3> getAllPositions()
	{
		List<Vector3> toReturn = new List<Vector3>();
		toReturn.Add(transform.position);
		toReturn.Add(transform.position + rightOffset);
		return toReturn;
	}

	public override void setBackBelt(ItemControl newIC)
	{
		if (newIC.transform.position == transform.position + backOffset)
			leftInput = newIC;
		else if (newIC.transform.position == transform.position + backOffset + rightOffset)
			rightInput = newIC;
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this);
		ItemControl behindRightSideIC = GetPlaceableAt<ItemControl>(grid, transform.position + backOffset + rightOffset);
		if (behindRightSideIC)
			TryAttachBackBeltHelper(grid, this, behindRightSideIC);
	}

	public override bool AllowBackBeltFrom(ItemControl askingIC)
	{
		if (askingIC.transform.position == transform.position + backOffset)
			if (leftInput == null)
				return true;
		if (askingIC.transform.position == transform.position + backOffset + rightOffset)
			if (rightInput == null)
				return true;
		return false;
	}

	public override void setBackBeltToNull(ItemControl deletingIC)
	{
		if (deletingIC.transform.position == transform.position + backOffset)
			leftInput = null;
		else if (deletingIC.transform.position == transform.position + backOffset + rightOffset)
			rightInput = null;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		// Try to move an item
		if (itemSlot && pullingIC && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlot, itemSlot.transform.position, pullingIC.transform.position));
			pullingIC.setItemSlot(this, itemSlot);
			base.itemSlot = null;
		}

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

	// If this belt is in front, start a chain reaction backwards for movement
	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null)
			MoveItem(null);
	}

	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, rightOffset);
		RemoveFromWorld(grid, this);

		if (leftInput)
		{
			leftInput.setFrontBeltToNull(this);
			leftInput.TryAttachFrontBelt();
		}
		leftInput = null;

		if (rightInput)
		{
			rightInput.setFrontBeltToNull(this);
			rightInput.TryAttachFrontBelt();
		}
		rightInput = null;

		if (frontBelt)
		{
			frontBelt.setBackBeltToNull(this);
			frontBelt.TryAttachBackBelt();
		}
		frontBelt = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}
}
