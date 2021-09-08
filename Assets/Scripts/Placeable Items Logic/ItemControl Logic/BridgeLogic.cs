using System;
using UnityEngine;

/* See Base Class for further documentation for all override functions */
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
			TryAttachOutputs();
		else if (newIC.transform.position == transform.position + transform.up)
			outputICs[0] = newIC;
		else if (newIC.transform.position == transform.position - transform.right)
			outputICs[1] = newIC;
	}
	public override void setInput(ItemControl newIC)
	{
		if (newIC == null)
			TryAttachInputs();
		else if (newIC.transform.position == transform.position - transform.up)
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
			MoveItemHelper(outputICs[0], 0, true, 0);
		else if (pullingIC == outputICs[1])
			MoveItemHelper(outputICs[1], 1, true, 1);
	}
	public override void BeltCycle(object sender, EventArgs e)
	{
		if (outputICs[0] == null)
			MoveItemHelper(outputICs[0], 0, true, 0);
		if (outputICs[1] == null)
			MoveItemHelper(outputICs[1], 1, true, 1);
	}
}