using System;
using UnityEngine;
using static HelpFuncs;
using static PlaceableHelpers;

public class MergerLeftLogic : ItemControl
{
	public GameObject rightToClone;
	MergerRightLogic rightPair;
	bool sideToMoveFromLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;

		// Needs to create and pair with SplitMergeRightLogic
		float rightSide = (transform.rotation.eulerAngles.z + 270) % 360;
		Vector3 pairPosition = transform.position + EulerToVector(rightSide);
		if (GetPlaceableAt<Placeable>(grid, pairPosition) != null)
		{
			base.RemovedAction();
			return;
		}

		GameObject tempChunkParent = GetChunkParentByPos(grid, pairPosition);
		GameObject tempPlaceable = Instantiate(rightToClone, pairPosition, transform.rotation, tempChunkParent.transform);
		rightPair = tempPlaceable.GetComponent<MergerRightLogic>();
		rightPair.PlacedAction(grid);
		rightPair.leftPair = this;

		grid.OnBeltTimerCycle += BeltCycle;
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
		TryAttachBackBeltHelper(grid, this);
	}

	// This is the part that needs to get reworked
	public override void MoveItem(ItemControl pullingIC)
	{
		// Try to move an item
		ItemControl sideToMoveFrom = chooseSideToMoveFrom();
		if (sideToMoveFrom && pullingIC && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, sideToMoveFrom.getItemSlot(this), sideToMoveFrom.getItemSlot(this).transform.position, frontBelt.transform.position));
			pullingIC.setItemSlot(this, sideToMoveFrom.getItemSlot(this));
			sideToMoveFrom.setItemSlot(this, null);
		}

		// Chain reaction backwards
		if (backBelt)
			backBelt.MoveItem(this);
		if (rightPair.getBackBelt())
			rightPair.getBackBelt().MoveItem(this);
	}

	// Chooses a random elible side of the SM to move an item from
	private ItemControl chooseSideToMoveFrom()
	{
		ItemControl sideToMoveFrom = null;
		if (sideToMoveFromLeft && itemSlot)
		{
			sideToMoveFrom = this;
			sideToMoveFromLeft = false;
		}
		else if (!sideToMoveFromLeft && rightPair.getItemSlot(this))
		{
			sideToMoveFrom = rightPair;
			sideToMoveFromLeft = true;
		}
		else if (itemSlot)
		{
			sideToMoveFrom = this;
			sideToMoveFromLeft = false;
		}
		else if (rightPair.getItemSlot(this))
		{
			sideToMoveFrom = rightPair;
			sideToMoveFromLeft = true;
		}
		return sideToMoveFrom;
	}

	// If this belt is in front, start a chain reaction backwards for movement
	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null)
			MoveItem(null);
	}

	public override void RemovedAction()
	{
		rightPair.leftPair = null;
		rightPair.RemovedAction();

		base.RemovedAction();
	}
}
