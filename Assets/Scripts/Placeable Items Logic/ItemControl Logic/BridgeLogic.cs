using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class BridgeLogic : ItemControl
{
	// topItem is the back to front path item, bottom is right to left path 
	GameObject topItem, bottomItem;
	// outputICs[0] is the front output, outputICs[1] is the left output
	// inputICs[0] is the back input, inputICs[1] is the right input

	// Placement in the world
	public override void PlacedAction(GridControl grid_)
	{
		inputICs = new ItemControl[2];
		outputICs = new ItemControl[2];
		inputValidRelPoses.Add(transform.right);
		outputValidRelPoses.Add(-transform.right);
		base.PlacedAction(grid_);
	}
	public override void RemovedAction()
	{
		base.RemovedAction();

		if (topItem)
			Destroy(topItem);
		topItem = null;

		if (bottomItem)
			Destroy(bottomItem);
		bottomItem = null;
	}

	public override void setOutput(ItemControl newIC)
	{
		if (newIC == null)
		{
			TryAttachOutputs();
			return;
		}
		int relativeAngle = getRelativeAngle(this, newIC);
		if (relativeAngle == 0)
			outputICs[0] = newIC;
		else if (relativeAngle == 270)
			outputICs[1] = newIC;
	}

	public override void setInput(ItemControl newIC)
	{
		if (newIC == null)
		{
			TryAttachInputs();
			return;
		}
		int relativeAngle = getRelativeAngle(this, newIC);
		if (relativeAngle == 180)
			inputICs[0] = newIC;
		else if (relativeAngle == 90)
			inputICs[1] = newIC;
	}

	// Item Stuff
	public override bool AllowItem(ItemControl askingIC)
	{
		if (askingIC == inputICs[0])
			return !topItem;
		else if (askingIC == inputICs[1])
			return !bottomItem;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			return !topItem;
	}
	public override void setItemSlot(ItemControl askingIC, GameObject item)
	{
		if (askingIC == inputICs[0])
			topItem = item;
		else if (askingIC == inputICs[1])
			bottomItem = item;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			topItem = item;
	}
	public override void MoveItem(ItemControl pullingIC)
	{
		if (pullingIC == outputICs[1])
			MoveItemLeftHelper();
		else if (pullingIC == outputICs[0])
			MoveItemFrontHelper();
	}
	private void MoveItemLeftHelper()
	{
		if (outputICs[1] && bottomItem && outputICs[1].AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, bottomItem, bottomItem.transform.position, outputICs[1].transform.position));
			outputICs[1].setItemSlot(this, bottomItem);
			bottomItem = null;
		}
		if (inputICs[1])
			inputICs[1].MoveItem(this);
	}
	private void MoveItemFrontHelper()
	{
		if (outputICs[0] && topItem && outputICs[0].AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, topItem, topItem.transform.position, outputICs[0].transform.position));
			outputICs[0].setItemSlot(this, topItem);
			topItem = null;
		}
		if (inputICs[0])
			inputICs[0].MoveItem(this);
	}
	public override void BeltCycle(object sender, EventArgs e)
	{
		if (outputICs[1] == null)
			MoveItemLeftHelper();
		if (outputICs[0] == null)
			MoveItemFrontHelper();
	}
}