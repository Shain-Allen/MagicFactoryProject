using System;
using UnityEngine;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class BridgeLogic : ItemControl
{
	// itemSlots[0] is the top item, back-to-front, itemSlots[1] is the bottom item, right-to-left
	// outputICs[0] is the front output, outputICs[1] is the left output
	// inputICs[0] is the back input, inputICs[1] is the right input

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
		if (newIC.transform.position == transform.position + transform.up)
			outputICs[0] = newIC;
		else if (newIC.transform.position == transform.position - transform.right)
			outputICs[1] = newIC;
	}
	public override void setInput(ItemControl newIC)
	{
		if (newIC == null)
		{
			TryAttachInputs();
			return;
		}
		if (newIC.transform.position == transform.position - transform.up)
			inputICs[0] = newIC;
		else if (newIC.transform.position == transform.position + transform.right)
			inputICs[1] = newIC;
	}

	// Item Stuff
	public override bool AllowItem(ItemControl askingIC)
	{
		if (askingIC == inputICs[0])
			return !itemSlots[0];
		else if (askingIC == inputICs[1])
			return !itemSlots[1];
		return false;
	}
	public override void setItemSlot(ItemControl askingIC, GameObject item)
	{
		if (askingIC == inputICs[0])
			itemSlots[0] = item;
		else if (askingIC == inputICs[1])
			itemSlots[1] = item;
	}
	public override void MoveItem(ItemControl pullingIC)
	{
		if (pullingIC == outputICs[0])
			MoveOneItem(0);
		else if (pullingIC == outputICs[1])
			MoveOneItem(1);
	}
	private void MoveOneItem(int side)
	{
		if (outputICs[side] && itemSlots[side] && outputICs[side].AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlots[side], itemSlots[side].transform.position, outputICs[side].transform.position));
			outputICs[side].setItemSlot(this, itemSlots[side]);
			itemSlots[side] = null;
		}
		if (inputICs[side])
			inputICs[side].MoveItem(this);
	}
	public override void BeltCycle(object sender, EventArgs e)
	{
		if (outputICs[1] == null)
			MoveOneItem(1);
		if (outputICs[0] == null)
			MoveOneItem(0);
	}
}