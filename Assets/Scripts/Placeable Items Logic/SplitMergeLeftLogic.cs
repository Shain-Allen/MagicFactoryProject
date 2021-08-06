using System;
using UnityEngine;
using static HelpFuncs;
using static PlaceableHelpers;

public class SplitMergeLeftLogic : ItemControl
{
	public GameObject rightToClone;
	SplitMergeRightLogic rightPair;

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
		bool leftFrontBelt, rightFrontBelt;
		ItemControl moveItemFromSM, outputBelt;

		while (itemSlot || rightPair.getItemSlot())
		{
			// Chooses a random elible side of the SM to move an item from
			if (itemSlot && rightPair.getItemSlot())
			{
				if (randgen.NextDouble() < .5)
					moveItemFromSM = this;
				else
					moveItemFromSM = rightPair;
			}
			else
			{
				if (itemSlot)
					moveItemFromSM = this;
				else
					moveItemFromSM = rightPair;
			}

			// Chooses a random elible front belt to move to
			leftFrontBelt = frontBelt && !frontBelt.getItemSlot();
			rightFrontBelt = rightPair.getFrontBelt() && !rightPair.getFrontBelt().getItemSlot();
			if (leftFrontBelt && rightFrontBelt)
			{
				if (randgen.NextDouble() < .5)
					outputBelt = frontBelt;
				else
					outputBelt = rightPair.getFrontBelt();
			}
			else if (leftFrontBelt)
				outputBelt = frontBelt;
			else if (rightFrontBelt)
				outputBelt = rightPair.getFrontBelt();
			else
				break;

			// Move the item
			moveItemFromSM.getItemSlot().transform.position = outputBelt.transform.position;
			outputBelt.setItemSlot(moveItemFromSM.getItemSlot());
			moveItemFromSM.setItemSlot(null);
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
