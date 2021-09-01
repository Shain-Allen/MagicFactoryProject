using System;
using UnityEngine;
using static HelpFuncs;
using static PlaceableHelpers;

public class MergerLogic : ItemControl
{
	ItemControl leftInput = null;
	ItemControl rightInput = null;
	GameObject leftItem = null;
	GameObject rightItem = null;
	Vector3 rightOffset;
	bool itemToMoveLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		rightOffset = EulerToVector(270 + transform.rotation.eulerAngles.z);
		if (GetPlaceableAt<Placeable>(grid, transform.position + rightOffset) != null)
			return;

		grid.OnBeltTimerCycle += BeltCycle;
		AddToWorld(grid, this);
		AddToWorld(grid, this, rightOffset);
		TryAttachFrontBelt();
		TryAttachBackBelt();
	}

	public override void setBackBelt(ItemControl newIC)
	{
		// TODO
		base.setBackBelt(newIC);
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this);
		Vector3 backSideOffset = EulerToVector(transform.rotation.eulerAngles.z + 180);
		ItemControl behindRightSideIC = GetPlaceableAt<ItemControl>(grid, transform.position + backSideOffset + rightOffset);
		if (behindRightSideIC)
			TryAttachBackBeltHelper(grid, this, behindRightSideIC);
	}

	public override bool AllowBackBeltFrom(ItemControl askingIC)
	{
		// TODO
		return base.AllowBackBeltFrom(askingIC);
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
