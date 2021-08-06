using System;
using UnityEngine;
using static HelpFuncs;
using static PlaceableHelpers;

public class SplitMergeLeftLogic : ItemControl
{
	public GameObject rightToClone;
	SplitMergeRightLogic rightPair;
	bool nextOutputLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;

		// Needs to create and pair with SplitMergeRightLogic
		float rightSide = (transform.rotation.eulerAngles.z + 270) % 360;
		Vector3 pairPosition = transform.position + EulerToVector(rightSide);
		if (GetPlaceableAt(grid, pairPosition) != null)
		{
			base.RemovedAction();
			return;
		}

		GameObject tempChunkParent = GetChunkParentByPos(grid, pairPosition);
		GameObject tempPlaceable = Instantiate(rightToClone, pairPosition, transform.rotation, tempChunkParent.transform);
		rightPair = tempPlaceable.GetComponent<SplitMergeRightLogic>();
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
		TryAttachBackBeltHelper(grid, this, 180);
	}

	// This is the part that needs to get reworked
	public override void MoveItem()
	{
		System.Random randgen = new System.Random();
		bool leftOutputFree, rightOutputFree;
		ItemControl sideToMoveFrom, outputBelt;

		while (itemSlot || rightPair.getItemSlot())
		{
			// Chooses a random elible side of the SM to move an item from
			if (itemSlot && rightPair.getItemSlot())
			{
				if (randgen.NextDouble() < .5)
					sideToMoveFrom = this;
				else
					sideToMoveFrom = rightPair;
			}
			else
			{
				if (itemSlot)
					sideToMoveFrom = this;
				else
					sideToMoveFrom = rightPair;
			}

			// Chooses a random elible front belt to move to
			outputBelt = null;
			leftOutputFree = frontBelt && !frontBelt.getItemSlot();
			rightOutputFree = rightPair.getFrontBelt() && !rightPair.getFrontBelt().getItemSlot();

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
			// If neither are free, stop!
			else
				break;

			// Move the item
			StartCoroutine(SmoothMove(grid, sideToMoveFrom.getItemSlot(), sideToMoveFrom.getItemSlot().transform.position, outputBelt.transform.position));
			outputBelt.setItemSlot(sideToMoveFrom.getItemSlot());
			sideToMoveFrom.setItemSlot(null);
		}

		// Chain reaction backwards
		if (backBelt)
			backBelt.MoveItem();
		if (rightPair.getBackBelt())
			rightPair.getBackBelt().MoveItem();
	}

	// If this belt is in front, start a chain reaction backwards for movement
	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null || rightPair.getFrontBelt() == null)
			MoveItem();
	}

	public override void RemovedAction()
	{
		rightPair.leftPair = null;
		rightPair.RemovedAction();

		base.RemovedAction();
	}
}
