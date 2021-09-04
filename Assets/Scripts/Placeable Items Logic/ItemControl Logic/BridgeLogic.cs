using System;
using UnityEngine;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class BridgeLogic : ItemControl
{
	// itemSlots[0] is the top item, back-to-front, itemSlots[1] is the bottom item, right-to-left
	// outputICs[0] is the front output, outputICs[1] is the left output
	// inputICs[0] is the back input, inputICs[1] is the right input

	// Placement in the world
	public override void PlacedAction(GridControl grid_)
	{
		inputICs = new ItemControl[2];
		outputICs = new ItemControl[2];
		itemSlots = new GameObject[2];
		inputValidRelPoses.Add(-transform.up);
		outputValidRelPoses.Add(transform.up);
		inputValidRelPoses.Add(transform.right);
		outputValidRelPoses.Add(-transform.right);
		base.PlacedAction(grid_);
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
			return !itemSlots[0];
		else if (askingIC == inputICs[1])
			return !itemSlots[1];
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			return !itemSlots[0];
	}
	public override void setItemSlot(ItemControl askingIC, GameObject item)
	{
		if (askingIC == inputICs[0])
			itemSlots[0] = item;
		else if (askingIC == inputICs[1])
			itemSlots[1] = item;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			itemSlots[0] = item;
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
		if (outputICs[1] && itemSlots[1] && outputICs[1].AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlots[1], itemSlots[1].transform.position, outputICs[1].transform.position));
			outputICs[1].setItemSlot(this, itemSlots[1]);
			itemSlots[1] = null;
		}
		if (inputICs[1])
			inputICs[1].MoveItem(this);
	}
	private void MoveItemFrontHelper()
	{
		if (outputICs[0] && itemSlots[0] && outputICs[0].AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlots[0], itemSlots[0].transform.position, outputICs[0].transform.position));
			outputICs[0].setItemSlot(this, itemSlots[0]);
			itemSlots[0] = null;
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