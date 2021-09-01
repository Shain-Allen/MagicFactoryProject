using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;
using static PlaceableHelpers;

public class MergerLogic : ItemControl
{
	ItemControl leftInput = null;
	ItemControl rightInput = null;
	GameObject leftItem = null;
	GameObject rightItem = null;
	Vector3 rightOffset, backOffset;
	bool itemToMoveLeft = true;

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

	public override void setItemSlot(ItemControl askingIC, GameObject item)
	{
		if (askingIC.transform.position == transform.position + backOffset)
			leftItem = item;
		else if (askingIC.transform.position == transform.position + backOffset + rightOffset)
			rightItem = item;
	}

	public override GameObject getItemSlot(ItemControl askingIC)
	{
		if (askingIC.transform.position == transform.position + backOffset)
			return leftItem;
		else if (askingIC.transform.position == transform.position + backOffset + rightOffset)
			return rightItem;
		return null;
	}

	public override bool AllowItem(Placeable askingPlaceable)
	{
		if (askingPlaceable.transform.position == transform.position + backOffset)
			return !leftItem;
		if (askingPlaceable.transform.position == transform.position + backOffset + rightOffset)
			return !rightItem;
		return false;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		// Try to move an item
		GameObject itemToMove = GetItemToMove();
		if (itemToMove && pullingIC && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemToMove, itemToMove.transform.position, pullingIC.transform.position));
			pullingIC.setItemSlot(this, itemToMove);
			if (itemToMoveLeft)
				rightItem = null;
			else
				leftItem = null;
		}

		// Chain reaction backwards
		if (leftInput)
			leftInput.MoveItem(this);
		if (rightInput)
			rightInput.MoveItem(this);
	}

	// Chooses a random elible side of the SM to move an item from
	private GameObject GetItemToMove()
	{
		if (itemToMoveLeft && leftItem)
		{
			itemToMoveLeft = false;
			return leftItem;
		}
		if (!itemToMoveLeft && rightItem)
		{
			itemToMoveLeft = true;
			return rightItem;
		}
		if (leftItem)
		{
			itemToMoveLeft = false;
			return leftItem;
		}
		if (rightItem)
		{
			itemToMoveLeft = true;
			return rightItem;
		}
		return null;
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
		base.RemovedAction();
	}
}
