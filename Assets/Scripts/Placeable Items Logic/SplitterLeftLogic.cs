using System;
using UnityEngine;
using static PlaceableHelpers;
using static HelpFuncs;

public class SplitterLeftLogic : ItemControl
{
	public GameObject rightToClone;
	SplitterRightLogic rightPair;
	bool nextOutputLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;

		// Needs to create and pair with SplitSplitterightLogic
		float rightSide = (transform.rotation.eulerAngles.z + 270) % 360;
		Vector3 pairPosition = transform.position + EulerToVector(rightSide);
		if (GetPlaceableAt<Placeable>(grid, pairPosition) != null)
		{
			base.RemovedAction();
			return;
		}

		GameObject tempChunkParent = GetChunkParentByPos(grid, pairPosition);
		GameObject tempPlaceable = Instantiate(rightToClone, pairPosition, transform.rotation, tempChunkParent.transform);
		rightPair = tempPlaceable.GetComponent<SplitterRightLogic>();
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
		ItemControl outputBelt = chooseOutputBelt();
		// Try to move the item
		if (itemSlot && outputBelt)
		{
			StartCoroutine(SmoothMove(grid, getItemSlot(this), getItemSlot(this).transform.position, outputBelt.transform.position));
			outputBelt.setItemSlot(this, getItemSlot(this));
			setItemSlot(this, null);
		}

		// Chain reaction backwards
		if (backBelt)
			backBelt.MoveItem(this);
		if (rightPair.getBackBelt())
			rightPair.getBackBelt().MoveItem(this);
	}

	// Tries to switch back and forth each time, if possible
	private ItemControl chooseOutputBelt()
	{
		ItemControl outputBelt = null;
		bool leftOutputFree = frontBelt && frontBelt.AllowItem(this);
		bool rightOutputFree = rightPair.getFrontBelt() && rightPair.getFrontBelt().AllowItem(this);

		// If it's left's turn and left is free, choose it
		if (nextOutputLeft && leftOutputFree)
		{
			outputBelt = frontBelt;
			nextOutputLeft = false;
		}
		// But if it's right's turn and right is free, choose it
		else if (!nextOutputLeft && rightOutputFree)
		{
			outputBelt = rightPair.getFrontBelt();
			nextOutputLeft = true;
		}
		// If it's right's turn but left is free, choose it
		else if (leftOutputFree)
		{
			outputBelt = frontBelt;
			nextOutputLeft = false;
		}
		// If it's left's turn but right is free, choose it
		else if (rightOutputFree)
		{
			outputBelt = rightPair.getFrontBelt();
			nextOutputLeft = true;
		}
		// If neither are free, will return null
		return outputBelt;
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
