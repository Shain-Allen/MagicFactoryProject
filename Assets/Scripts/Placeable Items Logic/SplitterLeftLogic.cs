using System;
using UnityEngine;
using static PlaceableHelpers;
using static HelpFuncs;

public class SplitterLeftLogic : ItemControl
{
	public GameObject rightToClone;
	SplitterRightLogic rightPair;
	bool sideToMoveFromLeft = true;
	bool nextOutputLeft = true;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;

		// Needs to create and pair with SplitSplitterightLogic
		float rightSide = (transform.rotation.eulerAngles.z + 270) % 360;
		Vector3 pairPosition = transform.position + EulerToVector(rightSide);
		if (GetPlaceableAt(grid, pairPosition) != null)
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
		TryAttachBackBeltHelper(grid, this, 180);
	}

	// This is the part that needs to get reworked
	public override void MoveItem()
	{
		ItemControl sideToMoveFrom, outputBelt;

		// This shouldn't be able to happen more than twice
		for (int i = 0; i < 2; i++)
		{
			sideToMoveFrom = chooseSideToMoveFrom();
			outputBelt = chooseOutputBelt();
			if (!sideToMoveFrom || !outputBelt)
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

	// Chooses a random elible side of the SM to move an item from
	private ItemControl chooseSideToMoveFrom()
	{
		ItemControl sideToMoveFrom = null;
		if (sideToMoveFromLeft && itemSlot)
		{
			sideToMoveFrom = this;
			sideToMoveFromLeft = false;
			Debug.Log("A 1");
		}
		else if (!sideToMoveFromLeft && rightPair.getItemSlot())
		{
			sideToMoveFrom = rightPair;
			sideToMoveFromLeft = true;
			Debug.Log("B 1");
		}
		else if (itemSlot)
		{
			sideToMoveFrom = this;
			sideToMoveFromLeft = false;
			Debug.Log("A 2");
		}
		else if (rightPair.getItemSlot())
		{
			sideToMoveFrom = rightPair;
			sideToMoveFromLeft = true;
			Debug.Log("B 2");
		}
		return sideToMoveFrom;
	}

	// Tries to switch back and forth each time, if possible
	private ItemControl chooseOutputBelt()
	{
		ItemControl outputBelt = null;
		bool leftOutputFree = frontBelt && !frontBelt.getItemSlot();
		bool rightOutputFree = rightPair.getFrontBelt() && !rightPair.getFrontBelt().getItemSlot();

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
