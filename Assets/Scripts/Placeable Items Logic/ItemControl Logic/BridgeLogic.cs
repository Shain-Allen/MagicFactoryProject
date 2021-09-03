using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class BridgeLogic : ItemControl
{
	// topItem is the back to front path item, bottom is right to left path 
	GameObject topItem, bottomItem;
	ItemControl backIn, frontOut, rightIn, leftOut;

	// Placement in the world
	public override void PlacedAction(GridControl grid_)
	{
		inputValidRelPoses.Add(transform.right);
		outputValidRelPoses.Add(-transform.right);
		base.PlacedAction(grid_);
	}
	public override void RemovedAction()
	{
		base.RemovedAction();

		if (backIn)
			backIn.TryAttachOutputs();
		backIn = null;

		if (rightIn)
			rightIn.TryAttachOutputs();
		rightIn = null;

		if (frontOut)
			frontOut.TryAttachInputs();
		frontOut = null;

		if (leftOut)
			leftOut.TryAttachInputs();
		leftOut = null;

		if (topItem)
			Destroy(topItem);
		topItem = null;

		if (bottomItem)
			Destroy(bottomItem);
		bottomItem = null;
	}

	public override void setOutput(ItemControl newIC)
	{
		int relativeAngle = getRelativeAngle(this, newIC);
		if (relativeAngle == 0)
			frontOut = newIC;
		else if (relativeAngle == 270)
			leftOut = newIC;
	}

	public override void setInput(ItemControl newIC)
	{
		int relativeAngle = getRelativeAngle(this, newIC);
		if (relativeAngle == 180)
			backIn = newIC;
		else if (relativeAngle == 90)
			rightIn = newIC;
	}

	// Item Stuff
	public override bool AllowItem(ItemControl askingIC)
	{
		if (askingIC == backIn)
			return !topItem;
		else if (askingIC == rightIn)
			return !bottomItem;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			return !topItem;
	}
	public override void setItemSlot(ItemControl askingIC, GameObject item)
	{
		if (askingIC == backIn)
			topItem = item;
		else if (askingIC == rightIn)
			bottomItem = item;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			topItem = item;
	}
	public override void MoveItem(ItemControl pullingIC)
	{
		if (pullingIC == leftOut)
			MoveItemLeftHelper();
		else if (pullingIC == frontOut)
			MoveItemFrontHelper();
	}
	private void MoveItemLeftHelper()
	{
		if (leftOut && bottomItem && leftOut.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, bottomItem, bottomItem.transform.position, leftOut.transform.position));
			leftOut.setItemSlot(this, bottomItem);
			bottomItem = null;
		}
		if (rightIn)
			rightIn.MoveItem(this);
	}
	private void MoveItemFrontHelper()
	{
		if (frontOut && topItem && frontOut.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, topItem, topItem.transform.position, frontOut.transform.position));
			frontOut.setItemSlot(this, topItem);
			topItem = null;
		}
		if (backIn)
			backIn.MoveItem(this);
	}
	public override void BeltCycle(object sender, EventArgs e)
	{
		if (leftOut == null)
			MoveItemLeftHelper();
		if (frontOut == null)
			MoveItemFrontHelper();
	}
}